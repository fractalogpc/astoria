using UnityEngine;

public class TEMPORARYTestHarvestable : MonoBehaviour
{
  public void Harvest(ItemData item) {
    Debug.Log($"Harvested {item.ItemName}");

    LocalPlayerReference.Instance.Inventory.TryAddItemsByData(item);
  }
}
