using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerInteractor : InputHandlerBase
{
  [SerializeField] private float _interactDistance = 4f;
  [SerializeField] private float _interactRadius = 0.2f;
  [SerializeField] private LayerMask _harvestableMask;
  [SerializeField] private FadeElementInOut _crosshairCanvas;
  [SerializeField] private ViewmodelManager _viewmodelManager;
  private Camera _camera;

  protected override void InitializeActionMap() {    
    RegisterAction(_inputActions.Player.Interact, _ => Interact());
  }

  public void Start() {
    _camera = Camera.main;
  }

  bool _showCrosshair = false;
  private void Update() {
    if (Physics.SphereCast(_camera.transform.position, _interactRadius, _camera.transform.forward, out RaycastHit hit, _interactDistance)) {
      Interactable interactable = hit.collider.GetComponentInParent<Interactable>();
      _showCrosshair = interactable != null;
    }
    else {
      _showCrosshair = false;
    }

    if (_showCrosshair) {
      if (_crosshairCanvas.FadingIn) return;
      _crosshairCanvas.FadeIn();
    }
    else {
      if (_crosshairCanvas.FadingOut) return;
      _crosshairCanvas.FadeOut();
    }
  }

  private void Interact() {
    _viewmodelManager.InteractAnimation();
    if (Physics.SphereCast(_camera.transform.position, _interactRadius, _camera.transform.forward, out RaycastHit hit, _interactDistance)) {
      Interactable interactable = hit.collider.GetComponentInParent<Interactable>();
      if (interactable != null) {
        interactable.Interact();
        return;
      }
    }
  }

}
