using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class DroppedItem : Interactable
{

  [HideInInspector] public ItemInstance Item;
  private bool _isBeingPickedUp = false;
  
  public override void Interact() {
    // Add the item to the player's inventory.
    InventoryComponent playerInventory = NetworkClient.localPlayer.gameObject.GetComponentInChildren<InventoryComponent>();
    if (playerInventory.AddItem(Item)) { 
      _isBeingPickedUp = true;
      Destroy(gameObject);
    }
  }

  private void OnDestroy() {
    if (_isBeingPickedUp) return;
    Item.OnItemDestruction();
  }
}