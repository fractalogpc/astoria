using UnityEngine;

namespace Construction
{
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
      return VectorFunctions.Vector3Approximately(pointA, otherEdge.pointA) && VectorFunctions.Vector3Approximately(pointB, otherEdge.pointB) || 
      VectorFunctions.Vector3Approximately(pointA, otherEdge.pointB) && VectorFunctions.Vector3Approximately(pointB, otherEdge.pointA);
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
  };
}