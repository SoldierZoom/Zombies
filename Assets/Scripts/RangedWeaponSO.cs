using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RangedWeaponSO:ScriptableObject {
    [SerializeField] private Transform prefab;
    [SerializeField] private string objectName;
    [SerializeField] private bool isOneHanded;

    public Transform GetPrefab() {
        return prefab;
    }
    public string GetObjectName() {
        return objectName;
    }
    public bool IsOneHanded() {
        return isOneHanded;
    }

}
