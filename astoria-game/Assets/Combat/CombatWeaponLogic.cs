using UnityEngine;
/// <summary>
/// This should be attached to an empty GameObject, to separate state. 
/// This handles the logic of a weapon: Raycasting for rifles, spawning projectiles for rocket launchers, etc
/// This class can be inherited from to create different weapon types.
/// </summary>
public class CombatWeaponLogic : MonoBehaviour
{
	public virtual void FireDown() {
		Debug.Log("Firing weapon");
	}
	public virtual void FireUp() {
		Debug.Log("Stopped firing weapon");
	}
	public virtual void ReloadDown() {
		Debug.Log("Reloading weapon");
	}
	public virtual void ADSStart() {
		Debug.Log("ADSing weapon");
	}
	public virtual void ADSEnd() {
		Debug.Log("Stopped ADSing weapon");
	}
	public virtual void Inspect() {
		Debug.Log("Inspecting weapon");	
	}
    
}
