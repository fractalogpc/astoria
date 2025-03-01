using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
	public static FMODEvents Instance { get; private set; }

	[Header("Player")]
	[field: SerializeField]
	public EventReference FootstepsEvent { get; private set; }

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}
}