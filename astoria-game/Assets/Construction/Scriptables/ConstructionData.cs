using System;
using UnityEngine;

public class ConstructionData : ScriptableObject
{
    public GameObject PreviewPrefab;
    public GameObject PlacedPrefab;

    [Header("Placement Offsets")]
    public ConstructionOffset Offset;

    [Header("Placement Settings")]
    public bool CanBePlacedOnGround;
    public bool CanBePlacedOnStructures;
    public bool CanBePlacedOnWalls;
    public bool CanBePlacedOnCeilings;
}

[Serializable]
public class ConstructionOffset {
    public float HeightOffset = 0;
    public Vector3 PositionOffset = Vector3.zero;
    public Vector3 RotationOffset = Vector3.zero;

    public Vector3 HeldPositionOffset = Vector3.zero;
    public Vector3 HeldRotationOffset = Vector3.zero;
}