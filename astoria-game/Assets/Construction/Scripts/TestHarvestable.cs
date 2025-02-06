using UnityEngine;
using Mirror;

public class TestHarvestable : MonoBehaviour
{
    public void GiveItem(ItemData item) {
        PlayerInstance.Instance.GetComponentInChildren<InventoryComponent>().AddItemByData(item);
    }
}
