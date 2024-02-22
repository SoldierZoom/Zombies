using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MeleeWeaponScriptObj:ScriptableObject {
    [SerializeField] private Transform prefab;
    [SerializeField] private string objectName;
    [SerializeField] private bool isOneHanded;
    [SerializeField] private int damage;

    public Transform GetPrefab() {
        return prefab;
    }
    public string GetObjectName() { 
        return objectName; 
    }
    public bool IsOneHanded() {
        return isOneHanded; 
    }
    public void Attack(Vector3 pos,Vector3 dirVector,LayerMask hittableLayerMask) {
        if(Physics.CapsuleCast(pos,pos,0.5f,dirVector,out RaycastHit hitInfo,2,hittableLayerMask)) {
            if(hitInfo.transform.gameObject.layer == 8) {
                hitInfo.transform.root.gameObject.GetComponent<Zombie>().Damage(damage);
                Debug.Log("Enenemy Hit!");
            }
        }
    }

}

