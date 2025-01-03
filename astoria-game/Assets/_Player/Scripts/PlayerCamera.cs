using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
  public class PlayerCamera : InputHandlerBase
  {
    public bool canLook = true;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Transform CameraTransform;

    [HideInInspector] public Quaternion PlayerYLookQuaternion = Quaternion.identity;

    [HideInInspector] public float CameraXRotation = 0;
    private Vector2 _mouseInput;

    // Maximum vertical look angle (just below 90°)
    private const float MaxVerticalAngle = 89f;

    protected override void InitializeActionMap()
    {
      _actionMap = new Dictionary<InputAction, Action<InputAction.CallbackContext>>();

      RegisterAction(_inputActions.Player.Look, ctx => _mouseInput = ctx.ReadValue<Vector2>(), () => _mouseInput = Vector2.zero);
    }

    // Ok, this is really weird. Basically the PlayerController updates its Y rotation on a FixedUpdate loop, but this script updates the X rotation on a Update Loop.
    // In order to solve this, I store a local quaternion for the Player rotation that I update here, then fetch it when I need to update the PlayerController rotation.
    // This works, however it means that you can't rotate the PlayerController internally, instead you have to rotate it here.
    private void PlayerYLook()
    {
      PlayerYLookQuaternion *= Quaternion.AngleAxis(_mouseInput.x, Vector3.up);
    }

    private void CameraXLook()
    {
      CameraXRotation -= _mouseInput.y;
      if (CameraXRotation > 90) CameraXRotation = 90;
      if (CameraXRotation < -90) CameraXRotation = -90;

      CameraXRotation = Mathf.Clamp(CameraXRotation, -MaxVerticalAngle, MaxVerticalAngle);
    }

    private void LateUpdate()
    {
      if (!canLook) return;

      PlayerYLook();

      CameraXLook();
      Quaternion newRotation = Quaternion.Euler(CameraXRotation, _playerTransform.rotation.eulerAngles.y, 0);
      CameraTransform.SetPositionAndRotation(_cameraTarget.position, newRotation);
    }
  }
}
