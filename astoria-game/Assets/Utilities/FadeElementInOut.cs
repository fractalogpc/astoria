using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(CanvasGroup))]
public class FadeElementInOut : MonoBehaviour
{
  [Header("References")]
  public CanvasGroup _canvasGroup;
  [Header("Settings")]
  [SerializeField] private AnimationCurve _easingCurve;
  [SerializeField] private float _inDuration;
  [SerializeField] private float _outDuration;
  [Header("Events")]
  public UnityEvent _OnFadeComplete;

  private void OnValidate() {
    if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
  }

  /// <summary>
  /// Hides the element immediately.
  /// </summary>
  public void Hide() {
    _canvasGroup.alpha = 0;
    _canvasGroup.interactable = false;
    _canvasGroup.blocksRaycasts = false;
  }
  
  /// <summary>
  /// Shows the element immediately.
  /// </summary>
  public void Show() {
    _canvasGroup.alpha = 1;
    _canvasGroup.interactable = true;
    _canvasGroup.blocksRaycasts = true;
  }
  
  /// <summary>
  /// Fades in the element, allowing for a smooth transition.
  /// </summary>
  /// <param name="resetAlpha">Whether to start with the element fully hidden or at it's current opacity.</param>
  /// <returns>The fade in duration set on this script.</returns>
  public float FadeIn(bool resetAlpha = false) {
    _canvasGroup.interactable = true;
    _canvasGroup.blocksRaycasts = true;
    if (resetAlpha) {
      StartCoroutine(FadeElementInOutCoroutine(0, 1, _inDuration));
    } else {
      StartCoroutine(FadeElementInOutCoroutine(_canvasGroup.alpha, 1, _inDuration));
    }
    return _inDuration;
  }
  
  /// <summary>
  /// Fades out the element, allowing for a smooth transition.
  /// </summary>
  /// <param name="resetAlpha">Whether to start with the element fully visible or at it's current opacity.</param>
  /// <returns>The fade out duration set on this script.</returns>
  public float FadeOut(bool resetAlpha = false) {
    _canvasGroup.interactable = false;
    _canvasGroup.blocksRaycasts = false;
    if (resetAlpha) {
      StartCoroutine(FadeElementInOutCoroutine(1, 0, _outDuration));
    } else {
      StartCoroutine(FadeElementInOutCoroutine(_canvasGroup.alpha, 0, _outDuration));
    }
    return _outDuration;
  }
  private IEnumerator FadeElementInOutCoroutine(float startAlpha, float targetAlpha, float duration) {
    float time = 0;
    while (time < duration) {
      time += Time.unscaledDeltaTime;
      _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, _easingCurve.Evaluate(time / duration));
      yield return null;
    }
    _canvasGroup.alpha = targetAlpha;
    _OnFadeComplete?.Invoke();
  }
}
