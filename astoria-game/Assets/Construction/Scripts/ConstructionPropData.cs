using UnityEngine;

/// <summary>
/// Scriptable for construction props.
/// Props are things that can be placed anywhere. Think chairs, tables, lights, etc.
/// </summary>
[CreateAssetMenu(fileName = "Prop", menuName = "Scriptable Objects/ConstructionData/Prop")]
public class ConstructionPropData : ConstructionData
{
    [Header("Placement Settings")]
    public bool CanBePlacedOnGround;
    public bool CanBePlacedOnStructures;
    public bool CanBePlacedOnWalls;
    public bool CanBePlacedOnCeilings;
}