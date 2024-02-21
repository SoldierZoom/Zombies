using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponList : MonoBehaviour {
    List<MeleeWeaponScriptObj> meleeWeapons = new List<MeleeWeaponScriptObj>();
    [SerializeField] private MeleeWeaponScriptObj baseballBat, axe;

    List<RangedWeaponSO> rangedWeapons = new List<RangedWeaponSO>();
    [SerializeField] private RangedWeaponSO revolver;

    void Start() {
        //adding scribtable objects to list for melee weapon
        meleeWeapons.Add(baseballBat);
        meleeWeapons.Add(axe);
        //same for ranged weapons
        rangedWeapons.Add(revolver);
    }
    //finds scriptable object with weapon's name
    public MeleeWeaponScriptObj FindMeleeSO(string objectName) {
        for(int i = 0; i < meleeWeapons.Count; i++) {
            if(string.Equals(meleeWeapons[i].GetObjectName(),objectName)) {
                Debug.Log(meleeWeapons[i].GetObjectName());
                return meleeWeapons[i];
            }
        }
        return null;
    }
    public RangedWeaponSO FindRangedSO(string objectName) {
        for(int i = 0; i < rangedWeapons.Count; i++) {
            if(string.Equals(rangedWeapons[i].GetObjectName(),objectName)) {
                Debug.Log(rangedWeapons[i].GetObjectName());
                return rangedWeapons[i];
            }
        }
        return null;
    }
}
