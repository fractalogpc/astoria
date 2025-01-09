using UnityEngine;

public class ConstructionData : ScriptableObject
{
    public GameObject PreviewPrefab;
    public GameObject PlacedPrefab;

    [Header("Placement Offsets")]
    public float HeightOffset = 0;
    public Vector3 PositionOffset = Vector3.zero;
    public Vector3 RotationOffset = Vector3.zero;
}
