using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
  public class PlayerCamera : InputHandlerBase
  {
    public bool canLook = true;
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _cameraTarget;

    [HideInInspector] public Quaternion PlayerYLookQuaternion = Quaternion.identity;

    private float _cameraXRotation = 0;
    private Vector2 _mouseInput;

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
      _cameraXRotation -= _mouseInput.y;
      if (_cameraXRotation > 90) _cameraXRotation = 90;
      if (_cameraXRotation < -90) _cameraXRotation = -90;
    }

    private void LateUpdate()
    {
      if (!canLook) return;

      PlayerYLook();

      CameraXLook();
      Quaternion newRotation = Quaternion.Euler(_cameraXRotation, _transform.rotation.eulerAngles.y, 0);
      transform.SetPositionAndRotation(_cameraTarget.position, newRotation);
    }
  }
}
