using Mirror;
using UnityEngine;
/// <summary>
/// This should be attached to an empty GameObject, to separate state. 
/// This handles the logic of a weapon: Raycasting for rifles, spawning projectiles for rocket launchers, etc
/// This class can be inherited from to create different weapon types.
/// </summary>
public abstract class CombatWeaponLogic : NetworkedInputHandlerBase
{
	protected override void InitializeActionMap() {
		if (!isLocalPlayer) return;
		RegisterAction(_inputActions.Player.Attack, ctx => AttackDown(), () => AttackUp());
		RegisterAction(_inputActions.Player.AttackSecondary, ctx => AttackSecondaryDown(), () => AttackSecondaryUp());
		RegisterAction(_inputActions.Player.InspectItem, ctx => Inspect());
	}

	public abstract void AttackDown();
	public abstract void AttackUp();
	public abstract void AttackSecondaryDown();
	public abstract void AttackSecondaryUp();

	public virtual void Inspect() {
		Debug.Log("Inspecting weapon");	
	}
}
