// using System;
// using System.Collections;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
//
// public class AmmoUI : MonoBehaviour
// {
//   [SerializeField] private FadeElementInOut _ammoCanvas;
//   [SerializeField] private PlayerCombatCore _playerCombatCore;
//   [SerializeField] private TextMeshProUGUI _ammoText;
//   [SerializeField] private TextMeshProUGUI _weaponTypeText;
//   [SerializeField] private AnimationCurve _ammoChangeCurve;
//   [SerializeField] private float _ammoAnimateTime = 0.3f;
//   
//   private float _lastAmmo;
//   private bool _changingAmmo;
//   
//   private void Start() {
//     _playerCombatCore.AmmoChanged += UpdateAmmoCount;
//     _playerCombatCore._OnInventoryChanged.AddListener(OnInvChanged);
//     print("Ammo UI listener");
//     _ammoCanvas.FadeOut();
//   }
//   
//   private void OnInvChanged() {
//     print("Inventory changed");
//     WeaponInventorySlot slot = _playerCombatCore.GetCurrentlyEquippedWeaponSlot();
//     if (slot == null) {
//       _ammoCanvas.FadeOut();
//       return;
//     }
//     if (slot._weaponInstance.TryGetComponent<MeleeWeapon>(out _)) {
//       _weaponTypeText.text = "MELEE";
//     }
//     if (slot._weaponInstance.TryGetComponent<MagazineWeapon>(out MagazineWeapon mw)) {
//       switch (mw._fireType) {
//         case MagazineWeapon.FireTypes.SemiAuto:
//           _weaponTypeText.text = "SINGLE";
//           break;
//         case MagazineWeapon.FireTypes.Burst:
//           _weaponTypeText.text = "BURST";
//           break;
//         case MagazineWeapon.FireTypes.FullAuto:
//           _weaponTypeText.text = "AUTO";
//           break;
//         default:
//           _weaponTypeText.text = "ERROR";
//           break;
//       }
//     }
//     if (slot._weaponInstance.TryGetComponent<ThrowableWeapon>(out _)) {
//       _weaponTypeText.text = "THROWABLE";
//     }
//     _ammoCanvas.FadeIn();
//   }
//   
//   private void UpdateAmmoCount(int currentAmmo, int maxAmmo) {
//     _ammoText.text = $"{currentAmmo} / {maxAmmo}";
//     StopCoroutine(AnimateAmmoChange(_lastAmmo, currentAmmo, maxAmmo));
//     StartCoroutine(AnimateAmmoChange(_lastAmmo, currentAmmo, maxAmmo));
//     _lastAmmo = currentAmmo;
//   }
//   
//   private IEnumerator AnimateAmmoChange(float initalAmmo, float currentAmmo, float maxAmmo) {
//     _changingAmmo = true;
//     float time = 0;
//     while (time < _ammoAnimateTime) {
//       _ammoText.text = $"{Mathf.FloorToInt(Mathf.Lerp(initalAmmo, currentAmmo, _ammoChangeCurve.Evaluate(time)))} <sup>/ {maxAmmo.ToString()}</sup>";
//       time += Time.deltaTime;
//       yield return null;
//     }
//     _ammoText.text = $"{currentAmmo.ToString()} <sup>/ {maxAmmo.ToString()}</sup>";
//     _changingAmmo = false;
//   }
//   
//   private void OnDisable() {
//     // _playerCombatCore.AmmoChanged -= UpdateAmmoCount;
//     // _playerCombatCore._OnInventoryChanged.RemoveListener(OnInvChanged);
//   }
// }