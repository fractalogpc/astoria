using System;
using UnityEngine;

[Serializable]
public class ConstructionOffset {
    public float HeightOffset = 0;
    public Vector3 PositionOffset = Vector3.zero;
    public Vector3 RotationOffset = Vector3.zero;

    public Vector3 HeldPositionOffset = Vector3.zero;
    public Vector3 HeldRotationOffset = Vector3.zero;
}