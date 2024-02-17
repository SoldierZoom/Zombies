using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu()]
public class RangedWeaponSO:ScriptableObject {
    [SerializeField] private Transform prefab;
    [SerializeField] private string objectName;
    private int ammo,ammoInGun;
    [SerializeField] private int startingAmmo, maxAmmo, maxAmmoInGun, maxShootingRange;
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
    public float GetShootDelay() {
        return shootDelay;
    }
    //ammo functions
    public void IntialiseAmmo() {
        ammo = startingAmmo;
        ammoInGun = maxAmmoInGun;
    } 
    public void Shoot(Vector3 pos, Vector3 dirVector, LayerMask hittableLayer) {
        if(ammoInGun == 0) {
            Reload();
        } else {
            ammoInGun -= 1;
            if(Physics.Raycast(pos,dirVector,maxShootingRange,hittableLayer)) {
                //Debug.Log("Something got hit");
            } else {
                //Debug.Log("You can't shoot");
            }
        }
    }
    public void Reload() {
        Debug.Log("Before Reloading - Ammo: "+ammo+" Ammo in gun: "+ammoInGun);
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
        Debug.Log("After Reloading - Ammo: " + ammo + " Ammo in gun: " + ammoInGun);
    }

}
