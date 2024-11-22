using UnityEngine;

[CreateAssetMenu(fileName = "CombatWeaponData", menuName = "Scriptable Objects/CombatWeaponData")]
public class CombatWeaponData : ItemData
{
	public GameObject WeaponViewmodelPrefab;
	public GameObject WeaponLogicPrefab;
}