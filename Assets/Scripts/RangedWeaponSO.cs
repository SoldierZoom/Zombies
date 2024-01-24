using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RangedWeaponSO:ScriptableObject {
    [SerializeField] private Transform prefab;
    [SerializeField] private string objectName;
    private int ammo,ammoInGun;
    [SerializeField] private int maxAmmoInGun, maxShootingRange;
    [SerializeField] private float shootDelay;

    //get functions
    public Transform GetPrefab() {
        return prefab;
    }
    public string GetObjectName() {
        return objectName;
    }
    public int GetMaxAmmoInGun() {
        return maxAmmoInGun;
    }
    public int GetMaxShootingRange() {
        return maxShootingRange;
    }
    public float GetShootDelay() {
        return shootDelay;
    }
    //ammo functions
    public void Shoot() {
        ammoInGun -= 1;
        if(ammoInGun == 0) {
            Reload();
        }
    }
    public void Reload() {
        //checking if gun is already fully loaded and player has ammo to reload
        if (ammoInGun!=maxAmmoInGun&&ammo!=0) {
            //subtracting the amount that needs to be loaded from ammo
            ammo -= (maxAmmoInGun - ammoInGun);
            //checking if gun can be fully loaded
            if(ammo >= 0) {
                ammoInGun = maxAmmoInGun;
            }
            //if not partially loading gun with what is remaining
            else {
                ammoInGun = maxAmmoInGun + ammo;
                ammo = 0;
            }
        } 
    }

}
