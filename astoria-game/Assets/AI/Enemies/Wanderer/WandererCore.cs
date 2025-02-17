using System;
using UnityEngine;

public class WandererCore : MonoBehaviour
{
    [SerializeField] private WandererMovement _movement;
    [SerializeField] private VisionCone _vision;

    private Vector3 _lastPlayerPosition;
    
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
