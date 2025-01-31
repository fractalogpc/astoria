using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
/// </summary>
public class HarvestingMinigame : MonoBehaviour
{
    
    [Tooltip("The FadeElementInOut component attached to the parent element of all harvesting UI.")]
    [SerializeField] private FadeElementInOut _fadeElementInOut;
    [Tooltip("The UI component labeled ProgressBar under the harvesting UI element.")]
    [SerializeField] private Slider _progressBar;
    [Tooltip("The UI component labeled CritBar under the harvesting UI element.")]
    [SerializeField] private RectTransform _critBar;
    [Tooltip("The UI component labeled CritFillLeft under the CritBar UI element.")]
    [SerializeField] private Slider _critFillLeft;
    [Tooltip("The UI component labeled CritFillRight under the CritBar UI element.")]
    [SerializeField] private Slider _critFillRight;
    
    private class MinigameBar {
        public float CurrentValue;
        public float FullValue;
        public CritZone CritZone;
        public class CritZone {
            public float Start;
            public float End;
            
            public CritZone(float barMax, float sizePercent) {
                sizePercent;
            }
        }
        
        public MinigameBar(HealthInterface target, Collider targetCollider, float critZoneSizePercent) {
            CurrentValue = FullValue - target.CurrentHealth;
            FullValue = target.MaxHealth;
            CritZoneSize = critZoneSizePercent;
            if (CurrentValue >= FullValue) {
                IsInCritMode = true;
            }
            else {
                IsInCritMode = false;   
            }
        }
        public void Swing(float damage) {
            if (IsInCritMode) {
                CritSwing();
                return;
            }
            NormalSwing(damage);
        }

        private void NormalSwing(float damage) {
            CurrentValue += damage;
            if (CurrentValue <= FullValue) {
                IsInCritMode = true;
            }
        }
        private void CritSwing() {
            
        }
    }
    
    
    /// <summary>
    /// The Collider of the target harvestable object.
    /// </summary>
    private Collider _targetCollider;
    private MinigameBar _minigameBar;
    
    public void NewMinigame(HealthInterface target, Collider targetCollider, float critZoneSizePercent) {
        _fadeElementInOut.Show();
        _targetCollider = targetCollider;
        _minigameBar = new MinigameBar(target, targetCollider, critZoneSizePercent);
    }
    public void EndMinigame() {
        _fadeElementInOut.Hide();
        _targetCollider = null;
        _minigameBar = null;
    }
    public void Swing(float damage) {
        if (_minigameBar == null) return;
    }
    private bool IsLookingAtTarget() {
        if (_targetCollider == null) return false;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit)) {
            if (hit.collider == _targetCollider) {
                return true;
            }
        }
        return false;
    }
}
