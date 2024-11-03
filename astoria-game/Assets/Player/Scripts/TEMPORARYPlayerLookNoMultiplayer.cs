using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TEMPORARYPlayerLookNoMultiplayer : InputHandlerBase
{
  public bool canLook = true;
	[SerializeField] private Camera _camera;
	[SerializeField] private float _sensitivity = 1;
	[SerializeField] private float _cameraXRotation = 0;
	[SerializeField] private Transform _transform;

  private Vector2 _mouseInput;

	private void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

  protected override void InitializeActionMap() {
    _actionMap = new Dictionary<InputAction, Action<InputAction.CallbackContext>>();

    RegisterAction(_inputActions.Player.Look, ctx => _mouseInput = ctx.ReadValue<Vector2>(), () => _mouseInput = Vector2.zero);
  }

	private void Look() {
		Vector2 mouseInput = _mouseInput * _sensitivity;
		_transform.transform.Rotate(Vector3.up, mouseInput.x);
		_cameraXRotation -= mouseInput.y;
		if (_cameraXRotation > 90) _cameraXRotation = 90;
		if (_cameraXRotation < -90) _cameraXRotation = -90;
		_camera.transform.localRotation = Quaternion.Euler(_cameraXRotation, 0, 0);
	}
	
	private void Update() {
		if (Cursor.lockState == CursorLockMode.Locked && canLook) Look();
	}
}
