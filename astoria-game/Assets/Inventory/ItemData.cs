using UnityEngine;

/// <summary>
/// Item info that is set at design time.
/// </summary>
[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public string ItemDescription;
    public Sprite ItemIcon;
    [ColorUsage(true, false)] public Color ItemBGColor;
    public Vector2Int ItemSize;
}
