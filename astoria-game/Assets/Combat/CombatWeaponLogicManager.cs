
using System;
using UnityEngine;
/// <summary>
/// Manages Logic gameobjects in much the same way as CombatViewmodelManager manages Viewmodels.
/// </summary>
public class CombatWeaponLogicManager : MonoBehaviour
{
	[SerializeField] private Transform _weaponLogicParent;
	public GameObject SpawnWeaponLogic(GameObject weaponLogicPrefab) {
		return Instantiate(weaponLogicPrefab, _weaponLogicParent);
	}
	
}