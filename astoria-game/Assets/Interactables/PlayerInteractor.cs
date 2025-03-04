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

  private Camera _camera;

  protected override void InitializeActionMap() {    
    RegisterAction(_inputActions.Player.Interact, _ => Interact());
  }

  public void Start() {
    _camera = Camera.main;
  }

  private void Interact() {
    if (Physics.SphereCast(_camera.transform.position, _interactRadius, _camera.transform.forward, out RaycastHit hit, _interactDistance)) {
      Interactable interactable = hit.collider.GetComponentInChildren<Interactable>();
      if (interactable != null) {
        interactable.Interact();
        return;
      }
    }

    // // Try again with harvestables
    // if (Physics.SphereCast(_camera.transform.position, _interactRadius, _camera.transform.forward, out hit, _interactDistance, _harvestableMask)) {
    //   GroundHarvestable harvestable = hit.collider.transform.parent.gameObject.GetComponent<GroundHarvestable>();
    //   if (harvestable != null) {
    //     // TODO: Trigger collect animation
    //     (ItemData, int)[] harvestedItems = harvestable.Harvest();
    //     foreach ((ItemData item, int amount) in harvestedItems) {
    //       for (int i = 0; i < amount; i++) {
    //         PlayerInstance.Instance.GetComponentInChildren<InventoryComponent>().AddItemByData(item);
    //       }
    //     }
    //     return;
    //   }
    // }
  }

}
