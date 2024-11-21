using System.Collections.Generic;
using UnityEngine;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

[RequireComponent(typeof(Light))]
public class TimeCycleSunLight : MonoBehaviour
{
 	[SerializeField] private Light _directionalLight;
	private TimeCycleCore _timeCycleCore;
	
	private void Start() {
		_timeCycleCore = TimeCycleCore.Instance;
		_directionalLight = GetComponent<Light>();
	}

	private void Update() {
		if (_timeCycleCore._timeOfDay.DayLength == 0) return;
		_directionalLight.transform.rotation = Quaternion.Euler(_timeCycleCore._timeOfDay.SecsElapsed / _timeCycleCore._timeOfDay.DayLength * 360 - 90, -90f, 0);
	}
}