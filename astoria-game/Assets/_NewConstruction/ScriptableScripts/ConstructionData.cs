using UnityEngine;

public class ConstructionData : ScriptableObject {
    public GameObject PreviewPrefab;
    public GameObject PlacedPrefab;

    [Header("Placement Offsets")]
    public ConstructionOffset Offset;
}