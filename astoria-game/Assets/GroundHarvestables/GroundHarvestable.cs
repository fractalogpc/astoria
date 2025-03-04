using UnityEngine;
using System.Collections.Generic;

public class GroundHarvestable : Interactable
{
    
    [System.Serializable]
    public struct HarvestDropData
    {

        public ItemData item;
        public int minAmount;
        public int maxAmount;

    }

    [SerializeField] private HarvestDropData[] harvestDrops;

    public override void Interact()
    {
        base.Interact();
        (ItemData, int)[] harvestedItems = Harvest();
        foreach ((ItemData item, int amount) in harvestedItems) {
          for (int i = 0; i < amount; i++) {
            PlayerInstance.Instance.GetComponentInChildren<InventoryComponent>().AddItemByData(item);
          }
        }

        Destroy(gameObject);
    }

    public (ItemData, int)[] Harvest()
    {
        List<(ItemData, int)> items = new List<(ItemData, int)>();
        foreach (HarvestDropData drop in harvestDrops)
        {
            int amount = Random.Range(drop.minAmount, drop.maxAmount + 1);
            items.Add((drop.item, amount));
        }

        return items.ToArray();
    }

}
