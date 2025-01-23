using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerInteractor : InputHandlerBase, IStartExecution
{
  [SerializeField] private float _interactDistance = 4f;

  private Camera _camera;

  protected override void InitializeActionMap() {    
    RegisterAction(_inputActions.Player.Interact, _ => Interact());
  }

  public void InitializeStart() {
    _camera = Camera.main;
  }

  private void Interact() {
    if (!Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _interactDistance)) {
      print("did not hit anything");
      return;
    }
    Interactable interactable = hit.collider.GetComponentInChildren<Interactable>();
    interactable?.Interact();
  }
}
