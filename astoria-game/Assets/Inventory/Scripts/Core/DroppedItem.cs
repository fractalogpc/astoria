using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class DroppedItem : Interactable
{

  [HideInInspector] public ItemStack ItemStack;
  private bool _isBeingPickedUp = false;
  
  public override void Interact() {
    // Add the item to the player's inventory.
    InventoryComponent playerInventory = PlayerInstance.Instance.gameObject.GetComponentInChildren<InventoryComponent>();
    if (playerInventory.AddStack(ItemStack)) { 
      _isBeingPickedUp = true;
      Destroy(gameObject);
    }
  }

  private void OnDestroy() {
    if (_isBeingPickedUp) return;
    foreach (ItemInstance item in ItemStack.Items) {
      item.OnItemDestruction();
    }
  }
}