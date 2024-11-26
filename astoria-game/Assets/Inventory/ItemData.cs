using UnityEngine;

/// <summary>
/// Item info that is set at design time.
/// </summary>
[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
[System.Serializable]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public string ItemDescription;
    [Header("Inventory")]
    public Sprite ItemIcon;
    [ColorUsage(true, false)] public Color ItemBGColor;
    public Vector2Int ItemSize = Vector2Int.one;
    public bool IsStackable;
    [Header("Dropped Item")]
    public GameObject DroppedItemPrefab;
}
