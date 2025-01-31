using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Adds recoil to the camera; viewmodel sway/anim is handled by CombatViewmodel.
/// </summary>
public class CombatCameraRecoil : MonoBehaviour
{

    private class RecoilInstance
    {

        public AnimationCurve RecoilCurve;
        public float Duration;
        public float MagnitudeUpwards;
        public float MagnitudeHorizontal;
        public float MagnitudeBackwards;
        public float TimeElapsed;
        public float NoiseMagnitude;
        public float NoiseSpeed;

    }

    [SerializeField] private Transform _recoilTransform;

    [SerializeField] private AnimationCurve _recoilCurve;
    [SerializeField] private float _recoilMultiplier = 1;


    public static CombatCameraRecoil Instance { get; private set; }

    private List<RecoilInstance> _activeRecoilInstances = new List<RecoilInstance>();

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        float upwardsRecoil = 0;
        float horizontalRecoil = 0;
        float backwardsRecoil = 0;
        Debug.Log("Recoil instances: " + _activeRecoilInstances.Count);
        for (int i = 0; i < _activeRecoilInstances.Count; i++) {
            RecoilInstance recoilInstance = _activeRecoilInstances[i];
            recoilInstance.TimeElapsed += Time.deltaTime;
            float progress = recoilInstance.TimeElapsed / recoilInstance.Duration;
            if (progress >= 1) {
                _activeRecoilInstances.RemoveAt(i);
                i--;
                continue;
            }
            float recoilNoise = recoilInstance.NoiseMagnitude * (Mathf.PerlinNoise(Time.time * recoilInstance.NoiseSpeed, 1.2092f) - 0.5f) + 1;

            float recoil = recoilInstance.RecoilCurve.Evaluate(progress) * recoilNoise;
            upwardsRecoil += recoil * recoilInstance.MagnitudeUpwards;
            horizontalRecoil += recoil * recoilInstance.MagnitudeHorizontal;
            backwardsRecoil += recoil * recoilInstance.MagnitudeBackwards;
        }
        upwardsRecoil *= _recoilMultiplier;
        horizontalRecoil *= _recoilMultiplier;
        backwardsRecoil *= _recoilMultiplier;
        Debug.Log("Recoil: " + upwardsRecoil + ", " + horizontalRecoil + ", " + backwardsRecoil);
        _recoilTransform.localEulerAngles = new Vector3(-upwardsRecoil, horizontalRecoil, 0);
        _recoilTransform.localPosition = new Vector3(0, 0, backwardsRecoil);
    }

    public void ApplyRecoil(RecoilSettings recoilSettings) {
        RecoilInstance recoilInstance = new RecoilInstance {
            RecoilCurve = recoilSettings.RecoilCurve,
            Duration = recoilSettings.MeanRecoilTime + Random.Range(-recoilSettings.RecoilTimeVariation, recoilSettings.RecoilTimeVariation),
            MagnitudeUpwards = recoilSettings.MeanUpwardsRecoil + Random.Range(-recoilSettings.UpwardsRecoilVariation, recoilSettings.UpwardsRecoilVariation),
            MagnitudeHorizontal = recoilSettings.MeanHorizontalRecoil + Random.Range(-recoilSettings.HorizontalRecoilVariation, recoilSettings.HorizontalRecoilVariation),
            MagnitudeBackwards = recoilSettings.MeanBackwardsRecoil + Random.Range(-recoilSettings.BackwardsRecoilVariation, recoilSettings.BackwardsRecoilVariation),
            TimeElapsed = 0,
            NoiseMagnitude = recoilSettings.NoiseMagnitude,
            NoiseSpeed = recoilSettings.NoiseSpeed
        };
        _activeRecoilInstances.Add(recoilInstance);
    }

}
