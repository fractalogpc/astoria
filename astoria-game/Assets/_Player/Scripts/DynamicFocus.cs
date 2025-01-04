using UnityEngine;
using System.Collections;

public class DynamicFocus : MonoBehaviour
{

    [SerializeField] private float _minDistance = 5f;
    [SerializeField] private float _maxDistance = 10f;
    [SerializeField] private float focusSpeed = 0.15f;
    [SerializeField] private float _sphereCastRadius = 0.5f;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private Camera _camera;

    private float _targetDistance;
    private float _currentDistance;

    void Update() {
        if (Physics.SphereCast(_camera.transform.position, _sphereCastRadius, _camera.transform.forward, out RaycastHit hit, _maxDistance, _layerMask)) {
            SetFocus(hit.distance);
        } else {
            SetFocus(_maxDistance);
        }
        _currentDistance = Mathf.Lerp(_currentDistance, _targetDistance, focusSpeed);
        _camera.focusDistance = _currentDistance;
    }

    private void SetFocus(float distance) {
        _targetDistance = Mathf.Clamp(distance, _minDistance, _maxDistance);
    }

}
