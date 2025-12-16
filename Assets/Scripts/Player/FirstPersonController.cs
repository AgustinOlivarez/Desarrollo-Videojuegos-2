using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] Transform visual;

    [Header("Movement")]
    [SerializeField] float movementSpeed = 6f;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float jumpForce = 3f;
    private Vector2 _movement;
    private Vector3 _velocity;

    [Header("Camera")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] float mouseSensitivity = 0.05f;
    [SerializeField] float minLimit = -80f;
    [SerializeField] float maxLimit = 80f;
    private Vector2 _look;

    [Header("Crouch")]
    [SerializeField] float crouchHeight = 1f;
    [SerializeField] float standHeight = 2f;
    [SerializeField] float crouchSpeed = 1.2f;
    [SerializeField] float crouchLerpSpeed = 5f;
    private float targetHeight;
    private Vector3 targetCenter;
    private Vector3 targetVisualPos;
    private bool _isCrouching = false;

    [Header("Flashlight")]
    [SerializeField] private Light flashlight;
    private bool flashlightOn = true;

    private PlayerInputAction _inputAction;
    private CharacterController _characterController;

    private float _currentRotationY;
    private bool controlsEnabled = true;

    void Awake()
    {
        _inputAction = new PlayerInputAction();
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        // Bloqueo el cursor y lo oculto
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // Inicializo valores de crouch
        targetHeight = standHeight;
        targetCenter = new Vector3(0, standHeight / 2f, 0);
        targetVisualPos = new Vector3(0, standHeight / 2f, 0);
    }

    private void Update()
    {
        if (!controlsEnabled) return;

        Movement();
        Look();
        CrouchLerp();
    }

    private void OnEnable()
    {

        _inputAction.Player.Move.performed += SetMovement;
        _inputAction.Player.Move.canceled += ResetMovement;

        _inputAction.Player.Look.performed += SetLook;
        _inputAction.Player.Look.canceled += ResetLook;

        _inputAction.Player.Jump.performed += SetJump;

        _inputAction.Player.Crouch.performed += SetCrouch;
        _inputAction.Player.Crouch.canceled += ReleaseCrouch;

        _inputAction.Player.Flashlight.performed += SetFlashlight;

        GameManager.OnGamePaused += DisableControls;
        GameManager.OnGameStarted += EnableControls;
        GameManager.OnGameWin += DisableControls;
        GameManager.OnGameLose += DisableControls;
        GameManager.OnInfoControls += DisableControls;
        GameManager.OnGameResumed += EnableControls;

        EnableControls();
    }

    private void OnDisable()
    {
        _inputAction.Player.Disable();

        _inputAction.Player.Move.performed -= SetMovement;
        _inputAction.Player.Move.canceled -= ResetMovement;

        _inputAction.Player.Look.performed -= SetLook;
        _inputAction.Player.Look.canceled -= ResetLook;

        _inputAction.Player.Jump.performed -= SetJump;

        _inputAction.Player.Crouch.performed -= SetCrouch;
        _inputAction.Player.Crouch.canceled -= ReleaseCrouch;

        _inputAction.Player.Flashlight.performed -= SetFlashlight;

        GameManager.OnGamePaused -= DisableControls;
        GameManager.OnGameStarted -= EnableControls;
        GameManager.OnGameWin -= DisableControls;
        GameManager.OnGameLose -= DisableControls;
        GameManager.OnInfoControls -= DisableControls;
        GameManager.OnGameResumed -= EnableControls;
    }

    private void DisableControls()
    {
        controlsEnabled = false;
        _inputAction.Player.Disable();
        _movement = Vector2.zero;
        _look = Vector2.zero;
    }

    private void EnableControls()
    {
        controlsEnabled = true;
        _inputAction.Player.Enable();
    }


    private void SetMovement(InputAction.CallbackContext ctx)
    {
        _movement = ctx.ReadValue<Vector2>();
    }

    private void ResetMovement(InputAction.CallbackContext ctx)
    {
        _movement = Vector2.zero;
    }

    private void SetLook(InputAction.CallbackContext ctx)
    {
        _look = ctx.ReadValue<Vector2>();
    }

    private void ResetLook(InputAction.CallbackContext ctx)
    {
        _look = Vector2.zero;
    }

    private void SetJump(InputAction.CallbackContext ctx)
    {
        if (_characterController.isGrounded && !_isCrouching)
        {
            _velocity.y = jumpForce;
        }
    }

    private void SetCrouch(InputAction.CallbackContext ctx)
    {
        if (_isCrouching) return;
        if (!_characterController.isGrounded) return;
        _velocity.y = 0;

        targetHeight = crouchHeight;
        targetCenter = new Vector3(0, crouchHeight / 2f, 0);
        targetVisualPos = new Vector3(0, crouchHeight / 2f, 0);

        movementSpeed = crouchSpeed;
        _isCrouching = true;
    }

    private void ReleaseCrouch(InputAction.CallbackContext ctx)
    {
        if (!_isCrouching) return;
        _velocity.y = 0;

        targetHeight = standHeight;
        targetCenter = new Vector3(0, standHeight / 2f, 0);
        targetVisualPos = new Vector3(0, standHeight / 2f, 0);

        movementSpeed = 4f;
        _isCrouching = false;
    }

    private void SetFlashlight(InputAction.CallbackContext ctx)
    {
        ToggleFlashlight();
    }

    private void Movement()
    {
        Vector3 move = transform.right * _movement.x + transform.forward * _movement.y;

        Vector3 final = move * movementSpeed;

        if (_characterController.isGrounded && _velocity.y < 0)
        {
            if (_velocity.y < 0)
            {
                _velocity.y = -2f;
            }
        }

        _velocity.y += gravity * Time.deltaTime;
        final.y = _velocity.y;

        _characterController.Move(final * Time.deltaTime);
    }

    private void Look()
    {
        Vector2 mouseNormalized = _look * mouseSensitivity;
        _currentRotationY = Mathf.Clamp(_currentRotationY - mouseNormalized.y, minLimit, maxLimit);
        // Rota la camara
        cameraTransform.localRotation = Quaternion.Euler(_currentRotationY, 0f, 0f);
        transform.Rotate(Vector3.up * mouseNormalized.x);
    }

    private void CrouchLerp()
    {
        // Suavizado de crouch
        _characterController.height = Mathf.MoveTowards(_characterController.height, targetHeight, crouchLerpSpeed * Time.deltaTime);
        _characterController.center = Vector3.MoveTowards(_characterController.center, targetCenter, crouchLerpSpeed * Time.deltaTime);
        visual.localPosition = Vector3.MoveTowards(visual.localPosition, targetVisualPos, crouchLerpSpeed * Time.deltaTime);

        float scaleY = _characterController.height / standHeight;
        visual.localScale = new Vector3(1f, scaleY, 1f);

        Vector3 camPos = cameraTransform.localPosition;
        float targetCamY = _characterController.height - 0.3f;
        camPos.y = Mathf.MoveTowards(camPos.y, targetCamY, crouchLerpSpeed * Time.deltaTime);
        cameraTransform.localPosition = camPos;

        if (_characterController.isGrounded && _velocity.y > -2f)
        {
            _velocity.y = -2f;
        }

    }

    private void ToggleFlashlight()
    {
        flashlightOn = !flashlightOn;
        flashlight.enabled = flashlightOn;
    }


    private void OnDrawGizmos()
    {
        if (_characterController == null) return;
        Gizmos.color = _characterController.isGrounded ? Color.green : Color.red;
        // Punto donde el controller toca el suelo
        Vector3 spherePos = transform.position + _characterController.center - Vector3.up * (_characterController.height / 2f - _characterController.radius);
        Gizmos.DrawWireSphere(spherePos, _characterController.radius * 0.9f);
    }

}