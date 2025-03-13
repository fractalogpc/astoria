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

    public Edge()
    {
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

    // public bool IsSame(Vector3 tryPosition, Transform edgeTransform, float threshold = 0.1f)
    // {
    //   Vector3 edgePosition = edgeTransform.position + edgeTransform.rotation * position;

    //   return VectorFunctions.Vector3Approximately(tryPosition, edgePosition, threshold);
    // }

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
      Vector3 firstPosition = Transform.position + Transform.rotation * position;
      Vector3 secondPosition = otherEdge.Transform.position + otherEdge.Transform.rotation * otherEdge.position;
      return VectorFunctions.Vector3Approximately(firstPosition, secondPosition, threshold);
    }

    public bool IsSame(Vector3 tryPosition, float threshold = 0.1f)
    {
      Vector3 firstPosition = Transform.position + Transform.rotation * position;
      return VectorFunctions.Vector3Approximately(firstPosition, tryPosition, threshold);
    }

    // public bool IsSame(Edge otherEdge, Vector3 position1, Vector3 position2, Quaternion rotation1, Quaternion rotation2, float threshold = 0.1f)
    // {
    //   Vector3 edge1Point = position1 + rotation1 * position;
    //   Vector3 edge2Point = position2 + rotation2 * otherEdge.position;

    //   return VectorFunctions.Vector3Approximately(edge1Point, edge2Point, threshold);
    // }



    public float DistanceFromEdge(Vector3 tryPosition, float edgeWidth, float threshold)
    {
      Vector3 edgePosition = position;

      // Convert edge position to world space
      Vector3 worldEdgePosition = Transform.position + Transform.rotation * edgePosition;

      // Ignore the Y component of the edge position
      // tryPosition.y = worldEdgePosition.y;

      // Convert normal to world space and ensure it remains in the XZ plane
      Vector3 worldNormal = Transform.rotation * normal.normalized;
      worldNormal = new Vector3(worldNormal.x, 0, worldNormal.z).normalized;

      // Compute the edge's tangent direction (perpendicular to the normal in the XZ plane)
      Vector3 worldTangent = new Vector3(-worldNormal.z, 0, worldNormal.x); // Rotate 90 degrees

      // Compute the vector from the edge center to the point
      Vector3 toPoint = tryPosition - worldEdgePosition;

      // Project the point onto the tangent direction to find its relative position along the edge
      float tangentDistance = Vector3.Dot(toPoint, worldTangent);

      // Clamp the tangent distance to stay within the finite edge width
      float halfWidth = edgeWidth * 0.5f;
      float clampedTangentDistance = Mathf.Clamp(tangentDistance, -halfWidth, halfWidth);

      // Compute the closest point on the edge
      Vector3 closestPointOnEdge = worldEdgePosition + worldTangent * clampedTangentDistance;

      // Compute the actual signed distance from the closest point on the edge
      float signedDistance = Vector3.Distance(tryPosition, closestPointOnEdge);

      if (signedDistance > threshold)
      {
        return -1f;
      }

      return signedDistance;
    }



    public Vector3 WorldSpaceRotation()
    {
      Vector3 worldNormal = Transform.rotation * normal.normalized;

      // Debug.Log("World Normal: " + worldNormal);

      return worldNormal;
    }
    public Quaternion WorldSpaceRotationQuaternion()
    {
      return Quaternion.LookRotation(Transform.TransformDirection(normal));
    }

    public static (Vector3, Quaternion) SnapEdgeToEdge(Edge fromEdge, Edge toEdge, Transform fromEdgeTransform, Transform toEdgeTransform, bool flipNormal, bool useCameraRotation = false)
    {
      // Convert local positions and normals to world space
      Vector3 fromWorldPos = fromEdgeTransform.TransformPoint(fromEdge.position);
      Vector3 fromWorldNormal = fromEdgeTransform.rotation * fromEdge.normal.normalized;

      Vector3 toWorldPos = toEdgeTransform.TransformPoint(toEdge.position);
      Vector3 toWorldNormal = toEdgeTransform.rotation * toEdge.normal.normalized;

      // If using the camera rotation, select the normal that's closest to the camera's forward direction
      if (useCameraRotation)
      {
        Vector3 cameraForward = Camera.main.transform.forward; // This isn't great practice

        float forwardDot = Vector3.Dot(toWorldNormal, cameraForward);

        if (forwardDot < 0)
        {
          fromWorldNormal = -fromWorldNormal;
        }
      }

      // Flip the normal if needed
      if (flipNormal)
      {
        fromWorldNormal = -fromWorldNormal;
      }

      // Calculate rotation to align fromEdge normal opposite to toEdge normal
      Quaternion fullRotation = Quaternion.FromToRotation(fromWorldNormal, -toWorldNormal) * fromEdgeTransform.rotation;

      // Extract only the Y-axis rotation
      Vector3 eulerAngles = fullRotation.eulerAngles;
      Quaternion rotation = Quaternion.Euler(0, eulerAngles.y, 0); // Lock X and Z rotation

      // --- 🔄 Fix Position Offset Calculation ---
      // Rotate only the local position with the constrained rotation
      Vector3 rotatedFromPos = rotation * fromEdge.position;
      Vector3 rotatedFromWorldPos = fromEdgeTransform.position + rotatedFromPos;

      // Calculate the translation needed to align rotated fromEdge to toEdge
      Vector3 positionOffset = toWorldPos - rotatedFromWorldPos;

      // Final snapped position
      Vector3 finalPosition = fromEdgeTransform.position + positionOffset;

      return (finalPosition, rotation);
    }

  }
}