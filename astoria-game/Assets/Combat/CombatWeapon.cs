using UnityEngine;

public class CombatWeapon
{
    public GameObject WeaponViewmodel;
    public GameObject WeaponLogic;
    
    public CombatWeapon(WeaponData data, CombatViewmodelManager viewmodelManager, CombatWeaponLogicManager weaponLogicManager) {
        WeaponViewmodel = viewmodelManager.AddNewViewmodel(data.WeaponViewmodelPrefab);
        WeaponLogic = weaponLogicManager.SpawnWeaponLogic(data.WeaponLogicPrefab);
    }
}
