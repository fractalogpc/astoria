using UnityEngine;

public class ResetLocalTransform : MonoBehaviour
{
    
    public void Reset()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

}
