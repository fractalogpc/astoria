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
    public float MarkerPositionPercent => _minigameMarker.anchoredPosition.x / _minigameBar.rect.width;
    
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

    private bool _markerTargetRight;
    private bool _moveMarker;
    
    public void SetDisplayedBarTo(HarvestBar bar) {
        _progressBar.value = bar.CurrentValue / bar.MaxValue;
        _critFillLeft.value = bar.CritPosition - bar.CritWidth / 2;
        _critFillRight.value = 100 - bar.CritPosition - bar.CritWidth / 2;
        _minigameMarker.anchoredPosition = new Vector2(0, 0);
        switch (bar.State) {
            case HarvestBar.HarvestBarState.Progress:
                ShowProgressBar();
                break;
            case HarvestBar.HarvestBarState.Minigame:
                ShowMinigameBar();
                break;
            case HarvestBar.HarvestBarState.Hidden:
                HideBar(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

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
        if (!_moveMarker) return;
        _minigameMarker.anchoredPosition += Vector2.right * ((_markerTargetRight ? 1 : -1) * _critMarkerMoveSpeed * Time.deltaTime);
        if (_minigameMarker.anchoredPosition.x >= _minigameBar.rect.width) {
            _markerTargetRight = false;
        } else if (_minigameMarker.anchoredPosition.x <= 0) {
            _markerTargetRight = true;
        }
    }

    public void SetMarkerMovement(bool moving) {
        _moveMarker = moving;
    }
}
