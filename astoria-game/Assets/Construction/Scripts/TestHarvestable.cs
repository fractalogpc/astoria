using UnityEngine;
using Mirror;

public class TestHarvestable : MonoBehaviour
{
    public void GiveItem(ItemData item) {
        NetworkClient.localPlayer.GetComponentInChildren<InventoryComponent>().AddItemByData(item);
    }
}
