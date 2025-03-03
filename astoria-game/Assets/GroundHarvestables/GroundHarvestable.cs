using UnityEngine;
using System.Collections.Generic;

public class GroundHarvestable : MonoBehaviour
{
    
    [System.Serializable]
    public struct HarvestDropData
    {

        public ItemData item;
        public int minAmount;
        public int maxAmount;

    }

    [SerializeField] private HarvestDropData[] harvestDrops;

    public (ItemData, int)[] Harvest()
    {
        List<(ItemData, int)> items = new List<(ItemData, int)>();
        foreach (HarvestDropData drop in harvestDrops)
        {
            int amount = Random.Range(drop.minAmount, drop.maxAmount + 1);
            items.Add((drop.item, amount));
        }

        Destroy(gameObject);
        return items.ToArray();
    }

}
