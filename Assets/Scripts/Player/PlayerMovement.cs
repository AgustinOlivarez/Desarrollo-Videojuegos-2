using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float jumpForce = 3f;
    public float mouseSensitivity = 2f;
    public float smoothTime = 0.05f;
    public float cameraFollowSpeed = 10f;

    [Header("References")]
    public Transform playerCamera;
    public Rigidbody rb;
    public Transform headPoint;

    private float xRotation = 0f;
    private bool isGrounded;
    private bool isMoving;
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaVelocity;
    private Vector3 cameraVelocity = Vector3.zero;
    private Transform cameraTarget;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public Vector3 crouchHeadLocalPosition = new Vector3(0, 0.25f, 0); // aprox la mitad de la altura original
    public Vector3 standingHeadLocalPosition = new Vector3(0, 0.52f, 0);
    public float crouchSpeed = 1.5f;

    private bool isCrouching = false;
    private CapsuleCollider capsule;

    void Start()
    {
        rb.freezeRotation = true; // freeze a las rotaciones del rb 
        GameObject cameraPivot = new GameObject("CameraPivot");
        cameraTarget = cameraPivot.transform;
        cameraTarget.position = transform.position;
        capsule = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return; //Pausar totalmente el juego
        HandleCrouch();
        HandleMovement();
        HandleMouseLook();
        SmoothCameraFollow();
    }

    void HandleMovement()
    {
        float speed = isCrouching ? walkSpeed * 0.5f : walkSpeed; // velocidad definida con la velocidad de caminar si esta agachado o no
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calcula el movimiento en el plano XZ
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move *= speed;

        // Mantiene la velocidad vertical del Rigidbody (sin sobrescribirla)
        Vector3 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(move.x, currentVelocity.y, move.z);

        isMoving = isGrounded && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));

        // Salto
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 jumpVelocity = rb.linearVelocity;
            jumpVelocity.y = jumpForce;
            rb.linearVelocity = jumpVelocity;
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        Vector2 targetMouseDelta = new Vector2(mouseX, mouseY);
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, smoothTime);

        xRotation -= currentMouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * currentMouseDelta.x);
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) // Si tiene apretado el ctrl izquierdo se agacha
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))// Cuando suelta se levanta
        {
            StandUp();
        }
    }

    void SmoothCameraFollow()
    {
        if (headPoint == null) return;

        cameraTarget.position = Vector3.SmoothDamp(
            cameraTarget.position,
            headPoint.position,
            ref cameraVelocity,
            1f / cameraFollowSpeed
        );

        playerCamera.position = cameraTarget.position;
    }

    //Funcion para agacharse
    void Crouch()
    {
        isCrouching = true;
        capsule.height = crouchHeight;
        headPoint.localPosition = crouchHeadLocalPosition;
    }

    //Funcion para levantarse
    void StandUp()
    {
        isCrouching = false;
        capsule.height = standingHeight;
        headPoint.localPosition = standingHeadLocalPosition;
    }

    // Si toca el piso o sale del piso interactua con estos triggers
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

}