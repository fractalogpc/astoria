using System;
using UnityEngine;

public class RectAtWorldPosition : MonoBehaviour
{
    public Transform OriginTransform => _originTransform;
    [Tooltip("The transform that the rect will calculate distance relative to.")]
    [SerializeField] private Transform _originTransform;
    public Vector3 WorldPosition => _worldPosition;
    [Tooltip("The world position that the rect represents.")]
    [SerializeField] private Vector3 _worldPosition;
    [Tooltip("The rect transform that will be moved and resized.")]
    [SerializeField] private RectTransform _rectTransform;
    [Tooltip("The size of the rect when at or closer than the close distance.")]
    [SerializeField] private Vector2 _closeSizePx;
    [Tooltip("The size of the rect when at or beyond the far distance.")]
    [SerializeField] private Vector2 _farSizePx;
    [Tooltip("The distance at which the rect will be at its close size.")]
    [SerializeField] private float _closeDist;
    [Tooltip("The distance at which the rect will be at its far size.")]
    [SerializeField] private float _farDist;

    /// <summary>
    /// Set the world position that the rect will represent.
    /// </summary>
    /// <param name="worldPosition">The world position to set.</param>
    public void SetWorldPosition(Vector3 worldPosition) {
        _worldPosition = worldPosition;
    }
    
    /// <summary>
    /// Set the origin transform that the rect will calculate distance relative to.
    /// </summary>
    /// <param name="originTransform">The Transform to set.</param>
    public void SetOriginTransform(Transform originTransform) {
        _originTransform = originTransform;
    }
    
    private void Update() {
        float distance = Vector3.Distance(OriginTransform.position, WorldPosition);
        SetRectSize(_rectTransform, distance);
        SetRectPosition(_rectTransform, WorldPosition);
    }

    private void SetRectSize(RectTransform rect, float distance) {
        _rectTransform.sizeDelta = Vector2.Lerp(_closeSizePx, _farSizePx, Mathf.InverseLerp(_closeDist, _farDist, distance));
    }
    private void SetRectPosition(RectTransform rect, Vector3 worldPosition) {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);
        rect.position = screenPoint;
    }
}
