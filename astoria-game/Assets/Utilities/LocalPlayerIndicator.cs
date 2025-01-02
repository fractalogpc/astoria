using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

public class LocalPlayerIndicator : NetworkBehaviour
{
	[Header("IsLocalPlayer is only true for the top-level player object.\nReference this boolean in child objects.")]
	public bool IsLocalClientPlayer => isLocalPlayer;
	public UnityEvent OnLocalPlayerStart;
	public UnityEvent OnNetworkedPlayerStart;


	[SerializeField] public MonoBehaviour[] DisableForNetworkedPlayer;

	// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.
	private void Start() {
		if (isLocalPlayer) {
			OnLocalPlayerStart?.Invoke();
			Debug.Log($"{gameObject.name} is the local player. Running OnLocalPlayerStart.");
		}
		else {
			OnNetworkedPlayerStart?.Invoke();
			Debug.Log($"{gameObject.name} is a networked player. Running OnNetworkedPlayerStart. Disabling scripts.");
			DisableScriptsForNetworkedPlayer();
		}
	}

	public void DisableScriptsForNetworkedPlayer() {
		DisableForNetworkedPlayer = GetComponents<MonoBehaviour>();
		foreach (MonoBehaviour script in DisableForNetworkedPlayer) {
			if (script == this) continue;
			if (script.GetType() == typeof(NetworkTransformReliable)) continue;
			if (script.GetType() == typeof(NetworkIdentity)) continue;
			script.enabled = false;
		}
	}
}