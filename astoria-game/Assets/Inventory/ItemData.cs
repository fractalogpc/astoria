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
    public bool IsStackable;
    public GameObject DroppedItemPrefab;

    private void OnEnable() {
        if (ItemIcon == null) {
            ItemIcon = Resources.Load<Sprite>("DefaultItemAssets/NullImage");
        }
    }
    
    public virtual ItemInstance CreateItem() {
        return new ItemInstance(this);
    } 
}
