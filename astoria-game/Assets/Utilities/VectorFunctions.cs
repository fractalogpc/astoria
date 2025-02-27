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
        Vector3 line = point - pointOnLine;

        Vector3 lineProjection = Vector3.Project(line, planeNormal);

        Debug.DrawLine(pointOnLine, pointOnLine + lineProjection, Color.yellow);

        Debug.DrawLine(pointOnLine, point);

        return lineProjection.magnitude;
    }
}
