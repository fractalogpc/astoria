using UnityEngine;

public class PlayerControllerTemplate : InputHandlerBase
{
  #region Variables

  [Header("Player Controller Template")]
  [SerializeField] private float _walkSpeed = 5f;
  [SerializeField] private float _sprintSpeed = 8f;
  [SerializeField] private float _jumpForce = 5f;
  [SerializeField] private float _xSensitivity = 2f;
  [SerializeField] private float _ySensitivity = 2f;
  [SerializeField] private float _maxLookAngle = 90f;
  [SerializeField] private float _interactDistance = 2f;
  [SerializeField] private float _groundCheckRadius = 0.3f;
  [SerializeField] private string _groundTag = "Ground";
  [SerializeField] private Transform _groundCheckPoint;


  #endregion

  private float _currentSpeed;
  private Rigidbody _rb;
  private Camera _camera;

  private Vector2 _moveInput;
  private Vector2 _lookInput;
  private bool _isSprinting;
  private bool _isGrounded;
  private float _xRotation;


  protected override void SubscribeInputActions() {
    _inputActions.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
    _inputActions.Player.Move.canceled += _ => _moveInput = Vector2.zero;

    _inputActions.Player.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
    _inputActions.Player.Look.canceled += _ => _lookInput = Vector2.zero;

    _inputActions.Player.Sprint.performed += _ => _isSprinting = true;
    _inputActions.Player.Sprint.canceled += _ => _isSprinting = false;

    _inputActions.Player.Jump.performed += _ => Jump();

    _inputActions.Player.Attack.performed += _ => Attack();

    _inputActions.Player.Interact.performed += _ => Interact();
  }

  protected override void UnsubscribeInputActions() {
    _inputActions.Player.Move.performed -= ctx => _moveInput = ctx.ReadValue<Vector2>();
    _inputActions.Player.Move.canceled -= _ => _moveInput = Vector2.zero;

    _inputActions.Player.Look.performed -= ctx => _lookInput = ctx.ReadValue<Vector2>();
    _inputActions.Player.Look.canceled -= _ => _lookInput = Vector2.zero;

    _inputActions.Player.Sprint.performed -= _ => _isSprinting = true;
    _inputActions.Player.Sprint.canceled -= _ => _isSprinting = false;

    _inputActions.Player.Jump.performed -= _ => Jump();

    _inputActions.Player.Attack.performed -= _ => Attack();

    _inputActions.Player.Interact.performed -= _ => Interact();
  }

  protected override void Awake() {
    base.Awake();
    _rb = GetComponent<Rigidbody>();
  }

  private void Start() {
    _camera = ResourceHolder.Instance.MainCamera;
    _currentSpeed = _walkSpeed;
  }

  private void Update() {
    CheckGrounded();
  }

  private void LateUpdate() {
    Look();
  }

  private void FixedUpdate() {
    Move();
  }

  private void Look() {
    float mouseX = _lookInput.x * _xSensitivity * Time.deltaTime;
    float mouseY = _lookInput.y * _ySensitivity * Time.deltaTime;

    _xRotation -= mouseY;
    _xRotation = Mathf.Clamp(_xRotation, -_maxLookAngle, _maxLookAngle);

    // Rotate player and camera
    transform.Rotate(Vector3.up * mouseX);
    _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
  }

  private void Move() {
    _currentSpeed = _isSprinting ? _sprintSpeed : _walkSpeed;

    // Calculate movement direction based on input
    Vector3 moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
    moveDirection = transform.TransformDirection(moveDirection).normalized;

    // Apply force for movement, while preventing sliding by using velocity control
    Vector3 force = moveDirection * _currentSpeed;
    Vector3 velocity = _rb.linearVelocity;
    
    // Only apply forces to X and Z axes to prevent affecting the Y (gravity)
    _rb.linearVelocity = new Vector3(force.x, velocity.y, force.z);
  }

  private void Jump() {
    if (_isGrounded) {
      _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
  }

  private void CheckGrounded() {
  Collider[] colliders = Physics.OverlapSphere(_groundCheckPoint.position, _groundCheckRadius);
  
  _isGrounded = false;
  
  foreach (var collider in colliders) {
    if (collider.CompareTag(_groundTag)) {
      _isGrounded = true;
      break;
    }
  }
}
  private void Attack() {
    Debug.Log("Attack");
  }

  private void Interact() {
    Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, _interactDistance)) {
      Interactable interactable = hit.collider.GetComponent<Interactable>();

      if (interactable != null) {
        interactable.Interact();
      }
    }
  }
}
