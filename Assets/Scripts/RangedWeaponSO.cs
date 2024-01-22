using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RangedWeaponSO:ScriptableObject {
    [SerializeField] private Transform prefab;
    [SerializeField] private string objectName;
    private int ammoInGun;
    [SerializeField] private int maxAmmoInGun, maxShootingRange;

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

}
