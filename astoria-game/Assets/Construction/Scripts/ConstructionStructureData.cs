using System;
using UnityEngine;

/// <summary>
/// Scriptable for construction components.
/// Components are large-scale building items that make up a base. Think walls, floors, ceilings, etc.
/// </summary>
[CreateAssetMenu(fileName = "Component", menuName = "Scriptable Objects/ConstructionData/Component")]
[Serializable]
public class ConstructionComponentData : ConstructionData
{
    [Serializable]
    public enum StructureType
    {
        Foundation,
        Wall,
        Floor
    }

    [Header("Structure Settings")]
    public StructureType Type;

    public ItemSetList Cost;

}