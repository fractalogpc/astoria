using System;
using UnityEngine;

[Serializable]
public class WeaponInstance
{
    public WeaponData ConstantData;
    public GameObject WeaponViewmodelInstance;
    public GameObject WeaponLogicInstance;
    
    public WeaponInstance(WeaponData data) {
        ConstantData = data;
    }
    public WeaponInstance(WeaponData data, GameObject viewmodelInstance, GameObject weaponLogicInstance) {
        WeaponViewmodelInstance = viewmodelInstance;
        WeaponLogicInstance = weaponLogicInstance;
    }
}
