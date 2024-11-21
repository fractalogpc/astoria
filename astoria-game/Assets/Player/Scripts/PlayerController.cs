using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class PlayerController : InputHandlerBase, IStartExecution
{
  [SerializeField] private ExampleCharacterController _character;
  [SerializeField] private CharacterCamera _characterCamera;

  private Vector2 _movementInput;
  private Vector2 _cameraInput;
  private bool _jumpInput;

  protected override void InitializeActionMap() {
    _actionMap = new Dictionary<InputAction, Action<InputAction.CallbackContext>>();

    RegisterAction(_inputActions.Player.Move, ctx => _movementInput = ctx.ReadValue<Vector2>(), () => _movementInput = Vector2.zero);
    RegisterAction(_inputActions.Player.Look, ctx => _cameraInput = ctx.ReadValue<Vector2>(), () => _cameraInput = Vector2.zero);
    RegisterAction(_inputActions.Player.Jump, _ => _jumpInput = true, () => _jumpInput = false);
  }

  public void InitializeStart() {
    Cursor.lockState = CursorLockMode.Locked;
  }

  private void Update() {
    HandleCharacterInput();
  }

  private void LateUpdate() {
    HandleCameraInput();
  }

  private void HandleCameraInput() {
    Vector3 lookInputVector = new Vector3(_cameraInput.x, _cameraInput.y, 0f);
  
    if (Cursor.lockState != CursorLockMode.Locked) {
      lookInputVector = Vector3.zero;
    }

    _characterCamera.UpdateWithInput(lookInputVector);
  }

  private void HandleCharacterInput() {
    PlayerCharacterInputs characterInputs = new();

    characterInputs.MoveAxisForward = _movementInput.y;
    characterInputs.MoveAxisRight = _movementInput.x;
    characterInputs.CameraRotation = _characterCamera.Transform.rotation;
    characterInputs.JumpDown = _jumpInput;

    _character.SetInputs(ref characterInputs);
  }
}
