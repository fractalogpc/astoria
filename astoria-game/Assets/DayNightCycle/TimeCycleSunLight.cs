using System.Collections.Generic;
using UnityEngine;
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

	[SerializeField] private float _fullMoonIntensity = 1f;
	[SerializeField] private float _newMoonIntensity = 0.5f;

	private TimeCycleCore _timeCycleCore;
	
	private void Start() {
		_timeCycleCore = TimeCycleCore.Instance;
	}

	private void Update() {
		if (_timeCycleCore.TimeOfDay.DayLength == 0) return;
		_sunLight.transform.rotation = Quaternion.Euler(_timeCycleCore.TimeOfDay.SecsElapsed / _timeCycleCore.TimeOfDay.DayLength * 360 - 90, -90f, 0);
		_moonLight.transform.rotation = Quaternion.Euler(_timeCycleCore.TimeOfDay.SecsElapsed / _timeCycleCore.TimeOfDay.DayLength * 360 + 90, -90f, 0);

		// Calculate moon phase
		float moonPhase = _timeCycleCore.TimeOfDay.SecsElapsed / _timeCycleCore.TimeOfDay.DayLength % 29.5f / 29.5f;
		_moonLightData.moonPhase = moonPhase;
		_moonLight.intensity = Mathf.Lerp(_newMoonIntensity, _fullMoonIntensity, (moonPhase > 0.5f ? 1 - moonPhase : moonPhase) * 2);
	}
}