using Mirror;
using UnityEngine;

public class TEMPORARYTestHarvestable : MonoBehaviour
{
  public void Harvest(ItemData item) {
    Debug.Log($"Harvested {item.ItemName}");
    NetworkClient.localPlayer.gameObject.GetComponentInChildren<InventoryComponent>().AddItemByData(item);
    // GameObject dropped = Instantiate(item.DroppedItemPrefab);
    // dropped.transform.position = transform.position + Vector3.up * 3;
    // dropped.GetComponent<DroppedItem>().Item = item;
  }
}
