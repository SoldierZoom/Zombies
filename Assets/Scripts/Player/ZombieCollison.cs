using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCollison : MonoBehaviour{
    void OnCollisionStay(Collision collision) {
        Debug.Log(collision.gameObject.layer.ToString());
    }
}
