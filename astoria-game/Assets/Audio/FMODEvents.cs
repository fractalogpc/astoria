using UnityEngine;
using FMODUnity;

public class FMODEvents : Singleton<FMODEvents>
{
	[Header("Player")]
	[field: SerializeField] public EventReference FootstepsEvent { get; private set; }
}
