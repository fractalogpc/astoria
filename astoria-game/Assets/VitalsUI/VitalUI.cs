using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem.DualShock;

public class VitalUI : MonoBehaviour
{

  [Tooltip("The text element that displays the vital's current value.")]
  [SerializeField] private TextMeshProUGUI _vitalNumber;
  [Tooltip("The fill element that displays the vital's current value.")]
  [SerializeField] private Image _vitalFill;
  [Tooltip("An animation curve that defines the progression of the fill bar and number change.")]
  [SerializeField] private AnimationCurve _changeCurve;
  [Tooltip("The class whose vital this UI element will display.")]
  [SerializeField] private PlayerVitalHandler _vitalSource;
  [Tooltip("The time it takes for the vital to animate from one value to another.")]
  [SerializeField] private float _animateTime;
  
  private bool _changingHealth;
  private Coroutine _vitalAnimCoroutine;
  
  public void OnVitalHandlerChanged(float initial, float current, float max) {
    if (_changingHealth) StopCoroutine(_vitalAnimCoroutine);
    _vitalAnimCoroutine = StartCoroutine(AnimateChange(initial, current, max));
  }
  // Note: this will skip animation if more damage is received - I don't think that's a problem?

  private IEnumerator AnimateChange(float initial, float current, float max) {
    _changingHealth = true;
    float time = 0;
    _vitalNumber.text = $"{((int)current).ToString()}";
    while (time < _animateTime) {
      // Text animating can look a little weird, so I'm going to leave it out for now
      // _vitalNumber.text = $"{Mathf.FloorToInt(Mathf.Lerp(initial, current, _changeCurve.Evaluate(time)))} <sup>/ {((int)max).ToString()}</sup>";
      _vitalFill.fillAmount = Mathf.Lerp(initial / max, current / max, _changeCurve.Evaluate(time));
      time += Time.deltaTime;
      yield return null;
    }
    _vitalFill.fillAmount = current / max;
    _changingHealth = false;
  }

  private void Start() {
    _vitalSource.OnVitalChange += OnVitalHandlerChanged;
    _vitalNumber.text = $"{((int)_vitalSource.GetCurrentValue()).ToString()}";
    _vitalFill.fillAmount = _vitalSource.GetCurrentValue() / _vitalSource.GetMaxValue();
  }

  private void OnDisable() {
    _vitalSource.OnVitalChange -= OnVitalHandlerChanged;
  }
}