using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponList : MonoBehaviour {
    List<MeleeWeaponScriptObj> meleeWeapons = new List<MeleeWeaponScriptObj>();
    [SerializeField]private MeleeWeaponScriptObj baseballBat, axe;

    void Start() {
        //adding scribtable objects to list for melee weapon
        meleeWeapons.Add(baseballBat);
        meleeWeapons.Add(axe);
    }
    //finds scriptable object with weapon's name
    public MeleeWeaponScriptObj FindSO(string objectName) {
        for(int i = 0; i < meleeWeapons.Count; i++) {
            if(string.Equals(meleeWeapons[i].GetObjectName(),objectName)) {
                Debug.Log(meleeWeapons[i].GetObjectName());
                return meleeWeapons[i];
            }
        }
        return null;

    }
}
