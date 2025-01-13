using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class DroppedItem : Interactable
{

  [HideInInspector] public ItemInstance Item;

  public override void Interact() {
    // Add the item to the player's inventory.
    InventoryComponent playerInventory = NetworkClient.localPlayer.gameObject.GetComponentInChildren<InventoryComponent>();
    if (playerInventory.AddItem(Item)) {
      Destroy(gameObject);
    }
  }

  private void OnDestroy() {
    Item.OnItemDestruction();
  }
}