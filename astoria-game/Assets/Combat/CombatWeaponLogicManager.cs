
using System;
using Mirror;
using UnityEngine;
/// <summary>
/// Manages Logic gameobjects in much the same way as CombatViewmodelManager manages Viewmodels.
/// </summary>
public class CombatWeaponLogicManager : MonoBehaviour
{
	[SerializeField] private Transform _weaponLogicParent;
	[SerializeField] private GameObject _currentWeaponLogic;
	public GameObject AddWeaponLogic(GameObject weaponLogicPrefab) {
		GameObject newLogic = Instantiate(weaponLogicPrefab, _weaponLogicParent);
		newLogic.SetActive(false);
		return newLogic;
	}
	public GameObject GetCurrentWeaponLogic() {
		return _currentWeaponLogic;
	}
	public bool WeaponLogicInstanceExists(GameObject weaponLogicInstance) {
		foreach (Transform child in _weaponLogicParent) {
			if (child.gameObject == weaponLogicInstance) {
				return true;
			}
		}
		return false;
	}
	public bool SetCurrentWeaponLogicTo(GameObject weaponLogicInstance) {
		if (_weaponLogicParent.childCount > 0) {
			DisableAllWeaponLogic();
		}
		if (!WeaponLogicInstanceExists(weaponLogicInstance)) {
			Debug.LogError($"CombatWeaponLogicManager: Could not find weapon logic {weaponLogicInstance.name}.");
			return false;
		}
		weaponLogicInstance.SetActive(true);
		_currentWeaponLogic = weaponLogicInstance;
		return true;
	}
	public void DisableAllWeaponLogic() {
		foreach (Transform child in _weaponLogicParent) {
			child.gameObject.SetActive(false);
		}
	}
	
}