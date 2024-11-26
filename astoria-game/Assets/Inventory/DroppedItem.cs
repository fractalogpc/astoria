using UnityEngine;
using UnityEngine.UI;

public class DroppedItem : Interactable
{

  [HideInInspector] public ItemData Item;

  public override void Interact() {
    // Add the item to the player's inventory.
    if (!LocalPlayerReference.Instance.Inventory().TryAddItemsByData(Item)) return;
    // Destroy the game object.
    Destroy(gameObject);
  }

}