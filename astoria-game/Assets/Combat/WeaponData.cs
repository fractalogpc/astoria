using UnityEngine;

public enum WeaponType
{
	Melee,
	Pistol,
	Rifle,
	Sniper,
	Shotgun,
}

// TODO: Make a custom editor that changes the exposed fields based on the weapon type

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ItemData
{
	[Header("Weapon")]
	public WeaponType WeaponType;
	
	public GameObject ViewmodelPrefab;
	// public GameObject LogicPrefab;
	
	public override ItemInstance CreateItem() {
		return new WeaponInstance(this);
	}
}