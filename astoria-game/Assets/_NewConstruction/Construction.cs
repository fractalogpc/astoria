using sc.terrain.proceduralpainter;
using UnityEngine;

namespace Construction
{
  /// <summary>
  /// Struct for handling edges between construction components.
  /// </summary>
  [System.Serializable]
  public class Edge
  {
    public Vector3 position;
    public Vector3 normal;

    public Transform Transform;
    public ConstructionData Data;

    public bool usedHorizontally; // If another component is connected to this edge horizontally
    public bool usedVertically; // If another component is connected to this edge vertically

    public Edge() {
    }

    public Edge(Vector3 position, Vector3 normal, Transform transform, ConstructionData data, bool usedHorizontally = false, bool usedVertically = false)
    {
      this.position = position;
      this.normal = normal;
      this.Transform = transform;
      this.Data = data;
      this.usedHorizontally = usedHorizontally;
      this.usedVertically = usedVertically;
    }

    public bool IsSame(Vector3 tryPosition, Transform thisTransform, Transform edgeTransform, float threshold = 0.1f)
    {
      Vector3 edgePosition = edgeTransform.position + edgeTransform.rotation * position;

      Debug.Log($"EdgePoint: {tryPosition}, OtherPoint: {edgePosition}");

      return VectorFunctions.Vector3Approximately(tryPosition, edgePosition, threshold);
    }

    public void SetUsedHorizontally(bool usedHorizontally)
    {
      this.usedHorizontally = usedHorizontally;
    }

    public void SetUsedVertically(bool usedVertically)
    {
      this.usedVertically = usedVertically;
    }

    public bool IsSame(Edge otherEdge, float threshold = 0.1f)
    {
      return VectorFunctions.Vector3Approximately(position, otherEdge.position, threshold);
    }

    public bool IsSame(Edge otherEdge, Vector3 position1, Vector3 position2, Quaternion rotation1, Quaternion rotation2, float threshold = 0.1f)
    {
      Vector3 edge1Point = position1 + rotation1 * position;
      Vector3 edge2Point = position2 + rotation2 * otherEdge.position;

      return VectorFunctions.Vector3Approximately(edge1Point, edge2Point, threshold);
    }

    public float Distance(Vector3 tryPosition, Transform edgeParent)
    {
      return Vector3.Distance(edgeParent.TransformPoint(position), tryPosition);
      // return VectorFunctions.DistanceToLine(edgeParent.TransformPoint(position), edgeParent.rotation * normal, tryPosition);
    }

    public Vector3 WorldSpaceRotation(Transform edgeParent)
    {
      return edgeParent.rotation * normal;
    }
    public Quaternion WorldSpaceRotationQuaternion(Transform edgeParent)
    {
      return Quaternion.LookRotation(edgeParent.TransformDirection(normal));
    }

    public static (Vector3, Quaternion) SnapEdgeToEdge(Edge fromEdge, Edge toEdge, Transform fromEdgeTransform, Transform toEdgeTransform, bool flipRotation)
    {
      // Convert local positions and normals to world space
      Vector3 fromWorldPos = fromEdgeTransform.TransformPoint(fromEdge.position);
      Vector3 fromWorldNormal = fromEdgeTransform.rotation * fromEdge.normal.normalized;

      Vector3 toWorldPos = toEdgeTransform.TransformPoint(toEdge.position);
      Vector3 toWorldNormal = toEdgeTransform.rotation * toEdge.normal.normalized;

      // If flipRotation is true, flip the fromEdge normal
      if (flipRotation)
        fromWorldNormal = -fromWorldNormal;

      // Calculate rotation to align fromEdge normal opposite to toEdge normal
      Quaternion rotation = Quaternion.FromToRotation(fromWorldNormal, -toWorldNormal) * fromEdgeTransform.rotation;

      // Recalculate fromWorldPos after applying the new rotation
      Vector3 rotatedFromPos = rotation * (fromEdge.position); // Rotate local position
      Vector3 rotatedFromWorldPos = fromEdgeTransform.position + rotatedFromPos;

      // Calculate the translation needed to align rotated fromEdge to toEdge
      Vector3 positionOffset = toWorldPos - rotatedFromWorldPos;

      // Final snapped position
      Vector3 finalPosition = fromEdgeTransform.position + positionOffset;

      return (finalPosition, rotation);
    }
  }
}