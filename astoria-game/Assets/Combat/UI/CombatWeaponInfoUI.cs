using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatWeaponInfoUI : MonoBehaviour
{
  [SerializeField] private FadeElementInOut _infoFade;
  [SerializeField] private CombatCore _playerCombatCore;
  [SerializeField] private TextMeshProUGUI _weaponNameText;
  [SerializeField] private TextMeshProUGUI _ammoText;
  
  private void Start() {
    _playerCombatCore.OnEquipWeapon.AddListener(OnNewWeaponEquipped);
    _playerCombatCore.OnAmmoChanged.AddListener(UpdateAmmoCount);
  }

  private void OnDisable() {
    _playerCombatCore.OnEquipWeapon.RemoveListener(OnNewWeaponEquipped);
    _playerCombatCore.OnAmmoChanged.RemoveListener(UpdateAmmoCount);
  }

  private void OnNewWeaponEquipped(GunInstance weapon) {
    _infoFade.FadeIn(true);
    _weaponNameText.text = weapon.WeaponData.ItemName;
    UpdateAmmoCount(weapon.CurrentAmmo, weapon.GetMaxAmmo());
  }
  private void UpdateAmmoCount(int oldAmmo = 0, int newAmmo = 0) {
    int maxAmmo = _playerCombatCore.CurrentGunInstance.GetMaxAmmo();
    _ammoText.text = $"{newAmmo} / {maxAmmo}";
  }
}