using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : InputHandlerBase, IStartExecution
{
  [SerializeField] private float _interactDistance = 4f;

  private Camera _camera;

  protected override void InitializeActionMap() {
    _actionMap = new Dictionary<InputAction, Action<InputAction.CallbackContext>>();
    
    RegisterAction(_inputActions.Player.Interact, ctx => Interact());
  }

  public void InitializeStart() {
    _camera = ResourceHolder.Instance.MainCamera;
  }

  private void Interact() {
    Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
    if (Physics.Raycast(ray, out RaycastHit hit, _interactDistance)) {
      Interactable interactable = hit.collider.GetComponent<Interactable>();
      interactable?.Interact();
    }
  }
}
