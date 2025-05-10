using UnityEngine;

public class GivePlayerItemOnDeath : MonoBehaviour
{

    public ItemData itemToGive;

    public void GiveItem() {
        PlayerInstance.Instance.GetComponentInChildren<InventoryComponent>().AddItemByData(itemToGive);
    }
}
