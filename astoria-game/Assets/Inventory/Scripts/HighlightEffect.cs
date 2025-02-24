using System;
using System.Collections;
using UnityEngine;

public class HighlightEffect : MonoBehaviour
{
	[SerializeField] private CanvasGroup _overlayCanvasGroup;
	[Header("Can be driven by ClickableEvents")]
	[SerializeField] private ClickableEvents _clickableEvents;
	[SerializeField] private float _highlightAlpha = 0.5f;
	[SerializeField] private AnimationCurve _fadeCurve;
	[SerializeField] private AnimationCurve _pulseCurve;
	[SerializeField] private float _fadeDuration = 0.1f;
	
	private Coroutine _fadeCoroutine;
	
	private void Start() {
		_overlayCanvasGroup.alpha = 0;
		_overlayCanvasGroup.blocksRaycasts = false;
		_overlayCanvasGroup.interactable = false;
		if (_clickableEvents != null) {
			_clickableEvents.OnHoverOn.AddListener(FadeIn);
			_clickableEvents.OnHoverOff.AddListener(FadeOut);
		}
	}

	private void OnDisable() {
		if (_clickableEvents != null) {
			_clickableEvents.OnHoverOn.RemoveListener(FadeIn);
			_clickableEvents.OnHoverOff.RemoveListener(FadeOut);
		}
	}

	public void FadeIn() {
		if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
		_fadeCoroutine = StartCoroutine(FadeTo(_highlightAlpha));
	}

	public void FadeOut() {
		if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
		_fadeCoroutine = StartCoroutine(FadeTo(0));
	}

	public void Pulse() {
		_fadeCoroutine = StartCoroutine(PulseCoroutine());
	}
	
	private IEnumerator PulseCoroutine() {
		float elapsedTime = 0;
		while (elapsedTime < _fadeDuration) {
			elapsedTime += Time.deltaTime;
			_overlayCanvasGroup.alpha = _pulseCurve.Evaluate(elapsedTime / _fadeDuration);
			yield return null;
		}
		_overlayCanvasGroup.alpha = 0;
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