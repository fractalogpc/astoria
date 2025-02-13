using UnityEngine;
using Construction;

/// <summary>
/// Generic scriptable for things that can be constructed.
/// </summary>
public class ConstructionData : ScriptableObject {
    public GameObject PreviewPrefab;
    public GameObject PlacedPrefab;

    [Header("Placement Offsets")]
    public ConstructionOffset Offset;
}