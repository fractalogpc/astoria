using UnityEngine;

public class LockRotation : MonoBehaviour
{
    Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void Update()
    {
        transform.rotation = initialRotation;
    }
}
