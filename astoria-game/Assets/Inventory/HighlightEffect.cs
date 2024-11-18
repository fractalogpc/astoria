using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ClickableEvents))]
public class HighlightEffect : MonoBehaviour
{
    [SerializeField] private CanvasGroup _overlayCanvasGroup;
    [SerializeField] private ClickableEvents _clickableEvents;
    [SerializeField] private float _highlightAlpha = 0.5f;
    [SerializeField] private AnimationCurve _fadeCurve;
    [SerializeField] private float _fadeDuration = 0.1f;
    
    private Coroutine _fadeCoroutine;
    
    private void Start() {
        _overlayCanvasGroup.alpha = 0;
        _overlayCanvasGroup.blocksRaycasts = false;
        _overlayCanvasGroup.interactable = false;
        _clickableEvents.OnHoverOn.AddListener(FadeIn);
        _clickableEvents.OnHoverOff.AddListener(FadeOut);
    }
    private void OnDisable() {
        _clickableEvents.OnHoverOn.RemoveListener(FadeIn);
        _clickableEvents.OnHoverOff.RemoveListener(FadeOut);
    }

    private void FadeIn() {
        if (_fadeCoroutine != null) {
            StopCoroutine(_fadeCoroutine);
        }
        _fadeCoroutine = StartCoroutine(FadeTo(_highlightAlpha));
    }
    private void FadeOut() {
        if (_fadeCoroutine != null) {
            StopCoroutine(_fadeCoroutine);
        }
        _fadeCoroutine = StartCoroutine(FadeTo(0));
    }
    private IEnumerator FadeTo(float targetAlpha) {
        float elapsedTime = 0;
        float startAlpha = _overlayCanvasGroup.alpha;
        while (elapsedTime < _fadeDuration) {
            elapsedTime += Time.deltaTime;
            _overlayCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, _fadeCurve.Evaluate(elapsedTime / _fadeDuration));
            yield return null;
        }
        _overlayCanvasGroup.alpha = targetAlpha;
    }
}
