using System;
using UnityEngine;

/// <summary>
/// A base class that any weapon should inherit from. 
/// </summary>
[Serializable]
public class WeaponInstance : ItemInstance
{
    public WeaponData WeaponData => (WeaponData) ItemData;
    public WeaponType WeaponType;
    public GameObject WeaponViewmodelInstance;

    public WeaponInstance(WeaponData weaponData) : base(weaponData) {
        WeaponType = weaponData.WeaponType;
    }
    
    
}
