using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

public class IsLocalPlayer : NetworkBehaviour
{
	public UnityEvent IfLocalPlayer;

	/// <summary>
	/// Add your validation code here after the base.OnValidate(); call.
	/// </summary>
	protected override void OnValidate() {
		base.OnValidate();
	}

	// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.
	private void Start() {
		if (isLocalPlayer) {
			IfLocalPlayer?.Invoke();
		}
		Debug.Log($"{gameObject.name} IsLocalPlayer = {isLocalPlayer}");
	}
}