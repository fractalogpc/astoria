using System;
using UnityEngine;

/// <summary>
/// This class represents item data set at runtime. Inherit this class whenever you need new dev-time variables. Remember to also inherit ItemInstance.
/// </summary>
[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/Items/ItemData", order = 99)]
[System.Serializable]
public class ItemData : BaseItemData
{
    [Tooltip("The name of the item.")]
    public string ItemName = "New Item";
    [Tooltip("The description of the item. Use this to add story and tips on it's usage.")]
    [TextArea(4, 12)] public string ItemDescription = "Item Description";

    [Header("Inventory")]
    [Tooltip("The icon of the item. This will be used in the inventory and crafting UI.")]
    public Sprite ItemIcon;
    [Tooltip("The color here will be applied as a tint to the background of the item in the inventory.")]
    [ColorUsage(true, false)] public Color ItemBGColor;
    [Tooltip("The size of the item in the inventory in item slots. This is the size of the item in the inventory grid.")]
    public Vector2Int ItemSize = Vector2Int.one;
    [Tooltip("If 0 or 1, the item will not stack.")]
    public int MaxStackSize = 8;
    [Tooltip("This prefab will spawn when the item is dropped from an inventory. Make sure the prefab contains the DroppedItem script.")]
    public GameObject DroppedItemPrefab;
    private void OnEnable() {
        if (ItemIcon == null) {
            ItemIcon = Resources.Load<Sprite>("DefaultItemAssets/NullImage");
        }
        if (DroppedItemPrefab == null) {
            DroppedItemPrefab = Resources.Load<GameObject>("DefaultItemAssets/DefaultDroppedItemPrefab");
        }
    }
    
    public override ItemInstance CreateItem() {
        return new ItemInstance(this);
    }

    public bool Equals(ItemData other) {
        if (other == null) return false;
        if (other.ItemName != ItemName) return false;
        if (other.ItemDescription != ItemDescription) return false;
        if (other.ItemIcon != ItemIcon) return false;
        if (other.ItemBGColor != ItemBGColor) return false;
        if (other.ItemSize != ItemSize) return false;
        if (other.MaxStackSize != MaxStackSize) return false;
        if (other.DroppedItemPrefab != DroppedItemPrefab) return false;
        return true;
    }
}
