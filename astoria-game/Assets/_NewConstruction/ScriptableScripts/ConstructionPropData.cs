using UnityEngine;

/// <summary>
/// Scriptable for construction props
/// </summary>
[CreateAssetMenu(fileName = "Prop", menuName = "Scriptable Objects/Construction/Prop")]
public class ConstructionPropData : ConstructionData
{
    [Header("Placement Settings")]
    public bool CanBePlacedOnGround;
    public bool CanBePlacedOnStructures;
    public bool CanBePlacedOnWalls;
    public bool CanBePlacedOnCeilings;
}