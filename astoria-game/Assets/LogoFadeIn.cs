using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogoFadeIn : MonoBehaviour
{
    
    [SerializeField] private AnimationCurve _scaleCurve;
    [SerializeField] private AnimationCurve _alphaCurve;
    [SerializeField] private float _duration = 1.0f;
    public float outDuration = 0.2f;
    [SerializeField] private Image _logoImage; // Assuming the logo is an Image component

    public void StartFadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    public void StartFadeOut() {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 initialScale = transform.localScale;
        Color initialColor = _logoImage.color;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _duration);

            float scaleValue = _scaleCurve.Evaluate(t);
            float alphaValue = _alphaCurve.Evaluate(t);

            transform.localScale = initialScale * scaleValue;
            Color newColor = initialColor;
            newColor.a = alphaValue;
            _logoImage.color = newColor;

            yield return null;
        }

        // Ensure final state is set
        transform.localScale = initialScale * _scaleCurve.Evaluate(1f);
        Color finalColor = initialColor;
        finalColor.a = _alphaCurve.Evaluate(1f);
        _logoImage.color = finalColor;
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 initialScale = transform.localScale;
        Color initialColor = _logoImage.color;

        while (elapsedTime < outDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / outDuration);

            float scaleValue = _scaleCurve.Evaluate(1 - t);
            float alphaValue = _alphaCurve.Evaluate(1 - t);

            transform.localScale = initialScale * scaleValue;
            Color newColor = initialColor;
            newColor.a = alphaValue;
            _logoImage.color = newColor;

            yield return null;
        }

        // Ensure final state is set
        transform.localScale = initialScale * _scaleCurve.Evaluate(0f);
        Color finalColor = initialColor;
        finalColor.a = _alphaCurve.Evaluate(0f);
        _logoImage.color = finalColor;
    }

}
