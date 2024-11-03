using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerTemplate : InputHandlerBase, IStartExecution
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
  [SerializeField] private float _groundDrag = 5f;

  #endregion

  private float _currentSpeed;
  private Rigidbody _rb;
  private Camera _camera;

  private Vector2 _moveInput;
  private Vector2 _lookInput;
  private bool _isSprinting;
  private bool _isGrounded;
  private float _xRotation;

  private void Awake() {
    _rb = GetComponent<Rigidbody>();
    _rb.interpolation = RigidbodyInterpolation.Interpolate; // Smoother movement
    _rb.freezeRotation = true; // Prevents physics from affecting rotation
  }

  public void InitializeStart() {
    _camera = ResourceHolder.Instance.MainCamera;
    _currentSpeed = _walkSpeed;
  }

  protected override void InitializeActionMap() {
    _actionMap = new Dictionary<InputAction, Action<InputAction.CallbackContext>>();

    RegisterAction(_inputActions.Player.Move, ctx => _moveInput = ctx.ReadValue<Vector2>(), () => _moveInput = Vector2.zero);
    RegisterAction(_inputActions.Player.Look, ctx => _lookInput = ctx.ReadValue<Vector2>(), () => _lookInput = Vector2.zero);
    RegisterAction(_inputActions.Player.Sprint, ctx => _isSprinting = true, () => _isSprinting = false);
    RegisterAction(_inputActions.Player.Jump, ctx => Jump());
    RegisterAction(_inputActions.Player.Attack, ctx => Attack());
    RegisterAction(_inputActions.Player.Interact, ctx => Interact());
  }

  private void Update() {
    CheckGrounded();
    HandleDrag();
  }

  private void LateUpdate() {
    Look();
  }

  private void FixedUpdate() {
    Move();
  }

  private void HandleDrag() {
    // Apply ground drag to reduce sliding when grounded
    _rb.linearDamping = _isGrounded ? _groundDrag : 0f;
  }

  private void Look() {
    float mouseX = _lookInput.x * _xSensitivity * Time.deltaTime;
    float mouseY = _lookInput.y * _ySensitivity * Time.deltaTime;

    _xRotation -= mouseY;
    _xRotation = Mathf.Clamp(_xRotation, -_maxLookAngle, _maxLookAngle);

    transform.Rotate(Vector3.up * mouseX);
    _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
  }

  private void Move() {
    _currentSpeed = _isSprinting ? _sprintSpeed : _walkSpeed;

    Vector3 moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
    moveDirection = transform.TransformDirection(moveDirection).normalized;

    // Calculate target velocity
    Vector3 targetVelocity = moveDirection * _currentSpeed;

    // Calculate velocity change and apply it as a force
    Vector3 velocityChange = targetVelocity - new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
    _rb.AddForce(velocityChange, ForceMode.VelocityChange);
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
    if (Physics.Raycast(ray, out RaycastHit hit, _interactDistance)) {
      Interactable interactable = hit.collider.GetComponent<Interactable>();
      interactable?.Interact();
    }
  }
}
