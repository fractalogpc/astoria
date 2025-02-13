using UnityEngine;

/// <summary>
/// Scriptable for construction components.
/// Components are large-scale building items that make up a base. Think walls, floors, ceilings, etc.
/// </summary>
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