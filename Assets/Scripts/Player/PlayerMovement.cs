using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float jumpForce = 5f;
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
    public Vector3 crouchHeadLocalPosition = new Vector3(0, 0.25f, 0);
    public Vector3 standingHeadLocalPosition = new Vector3(0, 0.52f, 0);
    public float crouchSpeed = 1.5f;

    private bool isCrouching = false;
    private CapsuleCollider capsule;
    private float lastJumpTime; 

    [Header("Ground Check")]
    public float groundCheckDistance = 0.2f;
    public LayerMask groundMask;

    private const float JUMP_COOLDOWN = 0.2f;

    void Start()
    {
        rb.freezeRotation = true;
        GameObject cameraPivot = new GameObject("CameraPivot");
        cameraTarget = cameraPivot.transform;
        cameraTarget.position = transform.position;
        capsule = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lastJumpTime = Time.time - JUMP_COOLDOWN; 
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        CheckGround();
        HandleCrouch();
        HandleMouseLook();
        SmoothCameraFollow();

        if (isGrounded && Input.GetButtonDown("Jump") && Time.time > lastJumpTime + JUMP_COOLDOWN)
        {
            Jump();
            lastJumpTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void CheckGround()
    {
        Vector3 sphereOrigin = transform.position + capsule.center;

        sphereOrigin.y -= capsule.height / 2f - capsule.radius / 2f;

        float radius = capsule.radius * 0.9f;

        isGrounded = Physics.CheckSphere(sphereOrigin, radius, groundMask, QueryTriggerInteraction.Ignore);
    }

    void HandleMovement()
    {
        float currentSpeed = isCrouching ? crouchSpeed : walkSpeed;
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 targetVelocity = transform.right * moveX + transform.forward * moveZ;
        targetVelocity *= currentSpeed;

        Vector3 velocity = rb.linearVelocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;

        rb.linearVelocity = velocity;

        isMoving = (moveX != 0f || moveZ != 0f) && isGrounded;
    }

    void Jump()
    {
        isGrounded = false;

        Vector3 jumpVelocity = rb.linearVelocity;
        jumpVelocity.y = jumpForce;
        rb.linearVelocity = jumpVelocity;
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
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
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

    void Crouch()
    {
        if (isCrouching) return;
        isCrouching = true;
        capsule.height = crouchHeight;
        capsule.center = new Vector3(capsule.center.x, crouchHeight / 2f, capsule.center.z);
        headPoint.localPosition = crouchHeadLocalPosition;
    }

    void StandUp()
    {
        if (!isCrouching) return;

        isCrouching = false;
        capsule.height = standingHeight;
        capsule.center = new Vector3(capsule.center.x, standingHeight / 2f, capsule.center.z);
        headPoint.localPosition = standingHeadLocalPosition;
    }

    // Gizmos
    void OnDrawGizmos()
    {
        if (capsule == null)
        {
            capsule = GetComponent<CapsuleCollider>();
            
        }

        Debug.Assert(capsule != null, "PlayerMovement requiere un componente CapsuleCollider en este GameObject.");
        if (capsule == null) return;

        Vector3 sphereOrigin = transform.position + capsule.center;
        sphereOrigin.y -= capsule.height / 2f - capsule.radius / 2f;

        float radius = capsule.radius * 0.9f;

        if (Application.isPlaying)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
        }
        else
        {
            Gizmos.color = Color.white;
        }

        Gizmos.DrawWireSphere(sphereOrigin, radius);

        if (headPoint != null && playerCamera != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(headPoint.position, playerCamera.position);
        }
    }
}