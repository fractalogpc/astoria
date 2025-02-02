using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
/// </summary>
public class HarvestingMinigame : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _critMarkerMoveSpeed = 1f;
    [Header("References")]
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
    [Tooltip("The UI component labeled Marker under the CritBar UI element.")]
    [SerializeField] private RectTransform _critMarker;
    
    
}
