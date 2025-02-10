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
}
