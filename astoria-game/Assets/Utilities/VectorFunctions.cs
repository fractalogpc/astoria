using UnityEngine;

public static class VectorFunctions
{
    public static bool Vector3Approximately(Vector3 a, Vector3 b)
    {
        if (Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z))
        {
            return true;
        }
        return false;
    }

    public static bool Vector3Approximately(Vector3 a, Vector3 b, float tolerance)
    {
        if (Mathf.Abs(a.x - b.x) <= tolerance && Mathf.Abs(a.y - b.y) <= tolerance && Mathf.Abs(a.z - b.z) <= tolerance)
        {
            return true;
        }
        return false;
    }

    public static float DistanceToLine(Vector3 pointOnLine, Vector3 planeNormal, Vector3 point)
    {
        // Ensure the normal is normalized
        Vector3 n = planeNormal.normalized;

        // Vector from the line's point to the second point
        Vector3 pointToLine = point - pointOnLine;

        // Project out the component along the normal (this gives the projection onto the plane)
        Vector3 projected = pointToLine - Vector3.Dot(pointToLine, n) * n;

        // The distance is the magnitude of this projection
        return projected.magnitude;
    }
}
