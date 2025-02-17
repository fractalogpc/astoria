using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class VisionCone : MonoBehaviour
{
    [SerializeField] private MeshCollider _meshCollider;
    [SerializeField][Range(0, 180)] private float _horizontalAngle;
    [SerializeField] private float _verticalHeight;
    [SerializeField] private float _distance;
    [SerializeField] private float _eyeHeight;
    private List<Collider> _colliderContacts = new();
    private List<GameObject> _visibleObjects = new();
    
    public List<GameObject> VisibleObjects => _visibleObjects;
    
    private void OnValidate() {
        if (_meshCollider == null) _meshCollider = GetComponent<MeshCollider>();
        if (_verticalHeight < 0) _verticalHeight = 0;
        if (_distance < 0) _distance = 0;
        SetTriggerFrustum(_distance, _horizontalAngle * Mathf.Deg2Rad, _verticalHeight);
    }
    private void SetTriggerFrustum(float distance, float horiAngle, float vertHeight) {
        _meshCollider.isTrigger = true;
        _meshCollider.sharedMesh = null;
        _meshCollider.sharedMesh = CreateFrustum(distance, horiAngle, vertHeight);
    }
    private Mesh CreateFrustum(float distance, float horiAngle, float vertHeight) {
        Mesh mesh = new();
        mesh.name = "Vision Frustum";
        Vector3[] vertices = new Vector3[8];
        
        vertices[0] = new Vector3(0, -vertHeight / 2f, 0);
        vertices[1] = new Vector3(0, vertHeight / 2f, 0);
        // Left side (-X)
        vertices[2] = new Vector3(-Mathf.Sin(horiAngle / 2f) * distance, -vertHeight / 2f, Mathf.Cos(horiAngle / 2f) * distance);
        vertices[3] = new Vector3(-Mathf.Sin(horiAngle / 2f) * distance, vertHeight / 2f, Mathf.Cos(horiAngle / 2f) * distance);
        // Middle
        vertices[4] = new Vector3(0, -vertHeight / 2f, distance);
        vertices[5] = new Vector3(0, vertHeight / 2f, distance);
        // Right side (+X)
        vertices[6] = new Vector3(Mathf.Sin(horiAngle / 2f) * distance, -vertHeight / 2f, Mathf.Cos(horiAngle / 2f) * distance);
        vertices[7] = new Vector3(Mathf.Sin(horiAngle / 2f) * distance, vertHeight / 2f, Mathf.Cos(horiAngle / 2f) * distance);

        int[] triangles = {
            // Left side
            0, 2, 3, 0, 3, 1,
            // Right side
            0, 1, 6, 1, 7, 6,
            // Top
            1, 3, 5, 1, 5, 7,
            // Bottom
            0, 4, 2, 0, 6, 4,
            // Back
            2, 4, 3, 3, 4, 5, 5, 4, 6, 6, 7, 5,
        };
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        return mesh;
    }
    
    private bool IsColliderVisible(Collider col) {
        Vector3 direction = col.transform.position - (transform.position + Vector3.up * _eyeHeight);
        Physics.Raycast(transform.position + Vector3.up * _eyeHeight, direction, out RaycastHit hit, _distance);
        return hit.collider == col;
    }
    
    private void Update() {
        _visibleObjects.Clear();
        foreach (Collider col in _colliderContacts) {
            if (col == null) continue;
            if (IsColliderVisible(col)) {
                _visibleObjects.Add(col.gameObject);
            }
        }
    }
    private void OnTriggerStay(Collider other) {
        if (!_colliderContacts.Contains(other)) {
            _colliderContacts.Add(other);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (_colliderContacts.Contains(other)) {
            _colliderContacts.Remove(other);
        }
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * _eyeHeight, 0.1f);
    }
}
