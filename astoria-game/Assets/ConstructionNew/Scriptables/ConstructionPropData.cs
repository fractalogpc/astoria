using UnityEngine;

[CreateAssetMenu(fileName = "Prop", menuName = "Scriptable Objects/Construction/Prop")]
public class ConstructionPropData : ConstructionData
{
    [Header("Placeable Settings")]
    public bool CanBePlacedOnGround;
    public bool CanBePlacedOnStructures;
    public bool CanBePlacedOnWalls;
    public bool CanBePlacedOnCeilings;
    public bool CanBePlacedInAir;
}
