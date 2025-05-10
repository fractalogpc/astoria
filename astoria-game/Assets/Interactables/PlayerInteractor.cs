using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using TMPro;
using UnityEngine.Serialization;

public class PlayerInteractor : InputHandlerBase
{
  [SerializeField] private float _interactDistance = 4f;
  [SerializeField] private float _interactRadius = 0.2f;
  [SerializeField] private LayerMask _harvestableMask;
  [FormerlySerializedAs("_crosshairCanvas")] [SerializeField] private FadeElementInOut _interactCanvas;
  [SerializeField] private TextMeshProUGUI _interactText;
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
      if (interactable != null  && interactable.enabled) {
        _showCrosshair = true;
        _interactText.text = interactable.InteractText;
      } 
      else {
        _showCrosshair = false;
        _interactText.text = "";
      }
    }
    else {
      _showCrosshair = false;
      _interactText.text = "";
    }
    if (_showCrosshair) {
      if (_interactCanvas.FadingIn) return;
      _interactCanvas.FadeIn();
    }
    else {
      if (_interactCanvas.FadingOut) return;
      _interactCanvas.FadeOut();
    }
  }

  private void Interact() {
    if (!Physics.SphereCast(_camera.transform.position, _interactRadius, _camera.transform.forward, out RaycastHit hit, _interactDistance)) return;
    Interactable interactable = hit.collider.GetComponentInParent<Interactable>();
    if (interactable == null) return;
    if (!interactable.enabled) return;
    _viewmodelManager.InteractAnimation();
    interactable.Interact();
    return;
  }

}
