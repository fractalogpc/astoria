using System;
using UnityEngine;

/// <summary>
/// This class represents item data set at runtime. Inherit this class whenever you need new dev-time variables. Remember to also inherit ItemInstance.
/// </summary>
[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
[System.Serializable]
public class ItemData : ScriptableObject
{
    public string ItemName = "New Item";
    public string ItemDescription = "Item Description";

    [Header("Inventory")]
    public Sprite ItemIcon;
    [ColorUsage(true, false)] public Color ItemBGColor;
    public Vector2Int ItemSize = Vector2Int.one;
    /// <summary>
    /// Exposed for equality comparison only. Use StackLimit instead.
    /// </summary>
    public bool IsStackable;
    /// <summary>
    /// Exposed for equality comparison only. Use StackLimit instead.
    /// </summary>
    public int MaxStackSize = 8;
    public int StackLimit => IsStackable ? MaxStackSize : 1;

    public GameObject DroppedItemPrefab;
    private void OnEnable() {
        if (ItemIcon == null) {
            ItemIcon = Resources.Load<Sprite>("DefaultItemAssets/NullImage");
        }
        if (DroppedItemPrefab == null) {
            DroppedItemPrefab = Resources.Load<GameObject>("DefaultItemAssets/DefaultDroppedItemPrefab");
        }
    }
    
    public virtual ItemInstance CreateItem() {
        return new ItemInstance(this);
    }

    public bool Equals(ItemData other) {
        if (other == null) return false;
        if (other.ItemName != ItemName) return false;
        if (other.ItemDescription != ItemDescription) return false;
        if (other.ItemIcon != ItemIcon) return false;
        if (other.ItemBGColor != ItemBGColor) return false;
        if (other.ItemSize != ItemSize) return false;
        if (other.IsStackable != IsStackable) return false;
        if (other.DroppedItemPrefab != DroppedItemPrefab) return false;
        return true;
    }
}
