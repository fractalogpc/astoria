using System;
using UnityEngine;

public class ResetLocalTransform : MonoBehaviour
{
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    private void Start() {
        // Store the initial position and rotation
        _initialPosition = transform.localPosition;
        _initialRotation = transform.localRotation;
    }

    public void Restore() {
        transform.localPosition = _initialPosition;
        transform.localRotation = _initialRotation;
    }
    public void Reset()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

}
