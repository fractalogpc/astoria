using UnityEngine;

[CreateAssetMenu(fileName = "Structure", menuName = "Scriptable Objects/Construction/Structure")]
public class ConstructionStructureData : ConstructionData
{
    public enum StructureType
    {
        Foundation,
        Wall,
        Ceiling
    }

    [Header("Structure Settings")]
    public StructureType Type;

}