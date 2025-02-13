using UnityEngine;

[CreateAssetMenu(fileName = "Component", menuName = "Scriptable Objects/Construction/Component")]
public class ConstructionComponentData : ConstructionData
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