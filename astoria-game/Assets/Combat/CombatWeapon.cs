using System;
using UnityEngine;

[Serializable]
public class CombatWeapon
{
    public WeaponData ConstantData;
    public GameObject WeaponViewmodelInstance;
    public GameObject WeaponLogicInstance;
    
    public CombatWeapon(WeaponData data, GameObject viewmodelInstance, GameObject weaponLogicInstance) {
        WeaponViewmodelInstance = viewmodelInstance;
        WeaponLogicInstance = weaponLogicInstance;
    }
}
