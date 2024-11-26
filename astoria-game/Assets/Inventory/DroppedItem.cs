using UnityEngine;
using UnityEngine.UI;

public class DroppedItem : Interactable
{

  [SerializeField] private ItemData _item;

  public override void Interact() {
    // Add the item to the player's inventory.
    if (!LocalPlayerReference.Instance.Inventory().TryAddItemsByData(_item)) return;
    // Destroy the game object.
    Destroy(gameObject);
  }

}