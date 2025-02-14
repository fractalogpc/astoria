using UnityEngine;

namespace Construction
{
  /// <summary>
  /// Struct for handling edges between construction components.
  /// </summary>
  [System.Serializable]
  public struct Edge
  {
    public Vector3 pointA;
    public Vector3 pointB;

    public Edge(Vector3 pointA, Vector3 pointB)
    {
      this.pointA = pointA;
      this.pointB = pointB;
    }

    public readonly bool IsSame(Edge otherEdge)
    {
      return (VectorFunctions.Vector3Approximately(pointA, otherEdge.pointA, 0.1f) && VectorFunctions.Vector3Approximately(pointB, otherEdge.pointB, 0.1f)) ||
      (VectorFunctions.Vector3Approximately(pointA, otherEdge.pointB, 0.1f) && VectorFunctions.Vector3Approximately(pointB, otherEdge.pointA, 0.1f));
    }

    public readonly bool IsSame(Edge otherEdge, Vector3 position1, Vector3 position2, Quaternion rotation1, Quaternion rotation2)
    {
      Vector3 edge1PointA = position1 + rotation1 * pointA;
      Vector3 edge1PointB = position1 + rotation1 * pointB;
      Vector3 edge2PointA = position2 + rotation2 * otherEdge.pointA;
      Vector3 edge2PointB = position2 + rotation2 * otherEdge.pointB;

      return (VectorFunctions.Vector3Approximately(edge1PointA, edge2PointA, 0.1f) && VectorFunctions.Vector3Approximately(edge1PointB, edge2PointB, 0.1f)) ||
      (VectorFunctions.Vector3Approximately(edge1PointA, edge2PointB, 0.1f) && VectorFunctions.Vector3Approximately(edge1PointB, edge2PointA, 0.1f));
    }

    public static float CalculateEdgeDistance(Edge edge1, Edge edge2, Transform parent1, Transform parent2)
    {
      Vector3 edge1PointA = parent1.TransformPoint(edge1.pointA);
      Vector3 edge1PointB = parent1.TransformPoint(edge1.pointB);
      Vector3 edge2PointA = parent2.TransformPoint(edge2.pointA);
      Vector3 edge2PointB = parent2.TransformPoint(edge2.pointB);

      float distanceAA = Vector3.Distance(edge1PointA, edge2PointA);
      float distanceAB = Vector3.Distance(edge1PointA, edge2PointB);
      float distanceBA = Vector3.Distance(edge1PointB, edge2PointA);
      float distanceBB = Vector3.Distance(edge1PointB, edge2PointB);

      float distance = Mathf.Min(distanceAA, distanceAB) + Mathf.Min(distanceBA, distanceBB);

      return distance;
    }

    public static float CalculateEdgeDistance(Edge edge1, Edge edge2, Vector3 position1, Quaternion rotation1, Vector3 position2, Quaternion rotation2)
    {
      Vector3 edge1PointA = position1 + rotation1 * edge1.pointA;
      Vector3 edge1PointB = position1 + rotation1 * edge1.pointB;
      Vector3 edge2PointA = position2 + rotation2 * edge2.pointA;
      Vector3 edge2PointB = position2 + rotation2 * edge2.pointB;

      float distanceAA = Vector3.Distance(edge1PointA, edge2PointA);
      float distanceAB = Vector3.Distance(edge1PointA, edge2PointB);
      float distanceBA = Vector3.Distance(edge1PointB, edge2PointA);
      float distanceBB = Vector3.Distance(edge1PointB, edge2PointB);

      float distance = Mathf.Min(distanceAA, distanceAB) + Mathf.Min(distanceBA, distanceBB);

      return distance;
    }

    public static (Vector3, Quaternion) SnapEdgeToEdge(Edge edge1, Edge edge2, Transform parent1, Transform parent2)
    {
      Vector3 edge1PointA = parent1.TransformPoint(edge1.pointA);
      Vector3 edge1PointB = parent1.TransformPoint(edge1.pointB);
      Vector3 edge2PointA = parent2.TransformPoint(edge2.pointA);
      Vector3 edge2PointB = parent2.TransformPoint(edge2.pointB);

      // Determine which points should serve as the pivot (the closest points)
      (float, int, int)[] distances = new (float, int, int)[4];
      distances[0] = (Vector3.Distance(edge1PointA, edge2PointA), 0, 0);
      distances[1] = (Vector3.Distance(edge1PointA, edge2PointB), 0, 1);
      distances[2] = (Vector3.Distance(edge1PointB, edge2PointA), 1, 0);
      distances[3] = (Vector3.Distance(edge1PointB, edge2PointB), 1, 1);

      // Sort the distances
      System.Array.Sort(distances, (a, b) => a.Item1.CompareTo(b.Item1));

      // Get the closest points
      Vector3 pivot1 = distances[0].Item2 == 0 ? edge1PointA : edge1PointB;
      Vector3 pivot2 = distances[0].Item3 == 0 ? edge2PointA : edge2PointB;

      Vector3 endpoint1 = distances[0].Item2 == 0 ? edge1PointB : edge1PointA;
      Vector3 endpoint2 = distances[0].Item3 == 0 ? edge2PointB : edge2PointA;

      // Calculate the rotation
      Quaternion rotation = Quaternion.FromToRotation(endpoint1 - pivot1, endpoint2 - pivot2);

      // Calculate the position given the rotation
      Vector3 position = pivot2 - rotation * pivot1;

      return (position, rotation);
    }

    public static (Vector3, Quaternion) SnapEdgeToEdge(Edge edge1, Edge edge2, Vector3 position1, Quaternion rotation1, Vector3 position2, Quaternion rotation2, bool? snapDirection = null)
    {
      Vector3 edge1PointA = position1 + rotation1 * edge1.pointA;
      Vector3 edge1PointB = position1 + rotation1 * edge1.pointB;
      Vector3 edge2PointA = position2 + rotation2 * edge2.pointA;
      Vector3 edge2PointB = position2 + rotation2 * edge2.pointB;

      // Determine which points should serve as the pivot (the closest points)
      (float, int, int)[] distances = new (float, int, int)[4];
      distances[0] = (Vector3.Distance(edge1PointA, edge2PointA), 0, 0);
      distances[1] = (Vector3.Distance(edge1PointA, edge2PointB), 0, 1);
      distances[2] = (Vector3.Distance(edge1PointB, edge2PointA), 1, 0);
      distances[3] = (Vector3.Distance(edge1PointB, edge2PointB), 1, 1);

      // Sort the distances
      System.Array.Sort(distances, (a, b) => a.Item1.CompareTo(b.Item1));

      // Get the closest points
      Vector3 pivot1 = distances[0].Item2 == 0 ? edge1PointA : edge1PointB;
      Vector3 pivot2 = distances[0].Item3 == 0 ? edge2PointA : edge2PointB;
      Vector3 pivot1Local = distances[0].Item2 == 0 ? edge1.pointA : edge1.pointB;
      Vector3 pivot2Local = distances[0].Item3 == 0 ? edge2.pointA : edge2.pointB;

      Vector3 endpoint1 = distances[0].Item2 == 0 ? edge1PointB : edge1PointA;
      Vector3 endpoint2 = distances[0].Item3 == 0 ? edge2PointB : edge2PointA;
      Vector3 endpoint1Local = distances[0].Item2 == 0 ? edge1.pointB : edge1.pointA;
      Vector3 endpoint2Local = distances[0].Item3 == 0 ? edge2.pointB : edge2.pointA;

      // Calculate the rotation
      Quaternion rotation = Quaternion.FromToRotation(endpoint1 - pivot1, endpoint2 - pivot2);

      // Calculate the position given the rotation
      Vector3 pivot1Updated = pivot1 - position1;
      pivot1Updated = rotation * pivot1Updated + position1;
      Vector3 position = pivot2 - pivot1Updated;

      Vector3 travel1 = (pivot1Local + endpoint1Local) / 2;
      Vector3 travel2 = (pivot2Local + endpoint2Local) / 2;

      float dot = Vector3.Dot(rotation1 * rotation * travel1, rotation2 * travel2);


      if (snapDirection == null)
      {
        // Defualt state
        if (dot > 0)
        {
          // Swap pivot and endpoint
          Vector3 temp = pivot1;
          pivot1 = endpoint1;
          endpoint1 = temp;

          rotation = Quaternion.FromToRotation(endpoint1 - pivot1, endpoint2 - pivot2);
          pivot1Updated = pivot1 - position1;
          pivot1Updated = rotation * pivot1Updated + position1;
          position = pivot2 - pivot1Updated;
        }
      }
      else if (snapDirection == true)
      {
        // Snap in direction of camera
        // Do nothing
      }
      else if (snapDirection == false)
      {
        // Swap pivot and endpoint
        Vector3 temp = pivot1;
        pivot1 = endpoint1;
        endpoint1 = temp;

        rotation = Quaternion.FromToRotation(endpoint1 - pivot1, endpoint2 - pivot2);
        pivot1Updated = pivot1 - position1;
        pivot1Updated = rotation * pivot1Updated + position1;
        position = pivot2 - pivot1Updated;
      }

      return (position, rotation);
    }
  };
}