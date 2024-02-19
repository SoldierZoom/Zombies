using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu()]
public class RangedWeaponSO:ScriptableObject {
    [SerializeField] private Transform prefab;
    [SerializeField] private string objectName;
    private int ammo,ammoInGun;
    [SerializeField] private int startingAmmo, maxAmmo, maxAmmoInGun, maxShootingRange, reloadTime;
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
            if(Physics.Raycast(pos,dirVector,out RaycastHit hitInfo,maxShootingRange,hittableLayer)) {
                //Debug.Log("Something got hit");
                //checking if object hit is in the enemy layer
                //Debug.Log(hitInfo.transform.gameObject.layer.ToString());
                if(string.Equals(hitInfo.transform.gameObject.layer.ToString(),"8")) {
                    Debug.Log("Enemy hit!");
                }
            } else {
                //Debug.Log("You can't shoot");
            }
        }
    }
    public void Reload() {
        //temp reload delay
        Debug.Log("Reloading...");
        Thread.Sleep(reloadTime);
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
