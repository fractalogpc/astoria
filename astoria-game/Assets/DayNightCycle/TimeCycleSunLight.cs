using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

public class TimeCycleSunLight : MonoBehaviour
{
 	[SerializeField] private Light _sunLight;
	[SerializeField] private Light _moonLight;
	[SerializeField] private HDAdditionalLightData _moonLightData;

	[SerializeField] private AnimationCurve _sunTempCurve;
	[SerializeField] private float _sunIntensityMax = 15000f;
	[SerializeField] private float _sunIntensityMin = 5500f;

	[SerializeField] private float _fullMoonIntensity = 1f;
	[SerializeField] private float _newMoonIntensity = 0.5f;
	[SerializeField] private float _moonPhaseOffset = 0.5f;
	[SerializeField] private int _framesPerUpdate = 5;
	
	[Header("Fog Settings")]
	[SerializeField] private AnimationCurve _fogDistanceCurve;
	[SerializeField] private Volume _globalVolume;
	// 8 AM - peak fog, starting to go down
	// 2 PM - bottom fog, remains there
	// 8 PM - fog increasing
	// 2 AM - fog peak, remains there
	private TimeCycleCore _timeCycleCore;
	private int _frameCount = 0;
	private Fog _fog;

	private void Start() {
		_timeCycleCore = TimeCycleCore.Instance;
		_globalVolume.profile.TryGet(out _fog);
	}

	private void Update() {
		_frameCount++;
		
		if (_frameCount < _framesPerUpdate) return;
		_frameCount = 0;

		if (_timeCycleCore.TimeOfDay.DayLength == 0) return;
		_sunLight.transform.rotation = Quaternion.Euler(_timeCycleCore.TimeOfDay.SecsElapsed / _timeCycleCore.TimeOfDay.DayLength * 360 - 90, 20f, 0);
		_moonLight.transform.rotation = Quaternion.Euler(_timeCycleCore.TimeOfDay.SecsElapsed / _timeCycleCore.TimeOfDay.DayLength * 360 + 90, 20f, 0);

		// Calculate moon phase
		float moonPhase = (_timeCycleCore.TimeOfDay.SecsElapsed / _timeCycleCore.TimeOfDay.DayLength % 29.5f / 29.5f + _moonPhaseOffset) % 1;
		_moonLightData.moonPhase = moonPhase;
		_moonLight.intensity = Mathf.Lerp(_newMoonIntensity, _fullMoonIntensity, (moonPhase > 0.5f ? 1 - moonPhase : moonPhase) * 2);

		_sunLight.colorTemperature = _sunTempCurve.Evaluate(_timeCycleCore.TimeOfDay.SecsElapsed / _timeCycleCore.TimeOfDay.DayLength) * (_sunIntensityMax - _sunIntensityMin) + _sunIntensityMin;
		Debug.Log(_fogDistanceCurve.Evaluate(_timeCycleCore.TimeOfDay.SecsElapsed / _timeCycleCore.TimeOfDay.DayLength));
		_fog.meanFreePath.value = _fogDistanceCurve.Evaluate(_timeCycleCore.TimeOfDay.SecsElapsed / _timeCycleCore.TimeOfDay.DayLength);
	}
}