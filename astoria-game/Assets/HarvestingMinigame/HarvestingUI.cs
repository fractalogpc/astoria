using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
/// </summary>
public class HarvestingUI : MonoBehaviour
{
    public bool BarShown => _barFade.Visible;
    public float MarkerPositionPercent => _minigameMarker.anchoredPosition.x / _minigameBar.rect.width;
    public bool IsCrit => MarkerPositionPercent >= _critFillLeft.value && MarkerPositionPercent <= 1 - _critFillRight.value;
    [Header("Settings")]
    [SerializeField] private float _critMarkerMoveSpeed = 250f;
    [Header("References")]
    [Tooltip("The FadeElementInOut component attached to the parent element of all harvesting UI.")]
    [SerializeField] private FadeElementInOut _barFade;
    [Tooltip("The UI component labeled ProgressBar under the harvesting UI element.")]
    [SerializeField] private Slider _progressBar;
    [Tooltip("The UI component labeled CritBar under the harvesting UI element.")]
    [SerializeField] private RectTransform _minigameBar;
    [Tooltip("The UI component labeled CritFillLeft under the CritBar UI element.")]
    [SerializeField] private Slider _critFillLeft;
    [Tooltip("The UI component labeled CritFillRight under the CritBar UI element.")]
    [SerializeField] private Slider _critFillRight;
    [Tooltip("The UI component labeled Marker under the CritBar UI element.")]
    [SerializeField] private RectTransform _minigameMarker;
    
    public float ShowBar() {
        return _barFade.FadeIn();
    }
    public float HideBar(bool instant = false) {
        if (instant) {
            _barFade.Hide();
            return 0;
        } 
        return _barFade.FadeOut();
    }

    public void ShowProgressBar() {
        _minigameBar.gameObject.SetActive(false);
        _progressBar.gameObject.SetActive(true);
    }
    public void ShowMinigameBar() {
        _progressBar.gameObject.SetActive(false);
        _minigameBar.gameObject.SetActive(true);
    }

    private void Update() {

    }
}
