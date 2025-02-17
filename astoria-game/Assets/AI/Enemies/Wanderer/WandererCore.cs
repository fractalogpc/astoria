using System;
using UnityEngine;

public class WandererCore : MonoBehaviour, IDamageable
{
    [SerializeField] private float _fullHealth = 100.0f;
    [SerializeField] private float _health;
    [SerializeField] private WandererMovement _movement;
    [SerializeField] private VisionCone _vision;
    [SerializeField] private RagdollToggle _ragdoll;
    
    private Vector3 _lastPlayerPosition;
    
    public void TakeDamage(float damage, Vector3 hitPosition) {
        _health -= damage;
        if (_health <= 0) {
            _movement.Stop();
            _ragdoll.Ragdoll(true);
            _ragdoll.ApplyForce((transform.position - hitPosition).normalized * 1.0f);
            Destroy(gameObject, 5.0f);
        }
    }
    private void OnValidate() {
        _health = _fullHealth;
    }
    private void Update() {
        foreach (GameObject obj in _vision.VisibleObjects) {
            if (!obj.CompareTag("Player")) continue;
            _lastPlayerPosition = obj.transform.position;
            _movement.SetTarget(_lastPlayerPosition);
            _movement.Go();
            return;
        }
    }
}
