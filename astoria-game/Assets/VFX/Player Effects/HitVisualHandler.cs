using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class HitVisualHandler : MonoBehaviour
{
    [SerializeField] private Volume _globalVolume;
    [SerializeField] private AnimationCurve _intensityCurve;
    [SerializeField] private float _vignetteIntensity = 0.5f;
    [SerializeField] private float _duration = 0.5f;
    private PlayerHitEffect _hit;
    private Vignette _vignette;
    
    private void Start()
    {
        if (_globalVolume.profile.TryGet(out _vignette))
        {
            _vignette.active = false;
        }
        else
        {
            Debug.LogError("Vignette not found in the volume profile.");
        }
        if (_globalVolume.profile.TryGet(out _hit))
        {
            _hit.active = false;
        }
        else
        {
            Debug.LogError("Hit effect not found in the volume profile.");
        }
    }
    
    public void TriggerHitEffect()
    {
        if (_vignette != null)
        {
            _vignette.active = true;
            _vignette.intensity.Override(0);
            StartCoroutine(ApplyVignetteEffect());
        }
        if (_hit != null)
        {
            _hit.active = true;
            _hit.hit.Override(0);
            StartCoroutine(ApplyHitEffect());
        }
    }
    
    private IEnumerator ApplyVignetteEffect()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _duration;
            _vignette.intensity.Override(_intensityCurve.Evaluate(t) * _vignetteIntensity);
            yield return null;
        }
        _vignette.active = false;
    }
    private IEnumerator ApplyHitEffect()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _duration;
            _hit.hit.Override(_intensityCurve.Evaluate(t));
            yield return null;
        }
        _hit.active = false;
    }
}
