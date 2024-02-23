using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject zombie;
    [SerializeField] private Transform zombieSpawn;
    private float cooldown=7,i;
    void Start()
    {
        i = 0f;
    }

    // Update is called once per frame
    void Update() {
        if(i >= cooldown) {
            Debug.Log("Spawn");
            Instantiate(zombie,zombieSpawn.position,Quaternion.identity);
            i = 0f;
        } else {
            i += Time.deltaTime;
        }
    }
}
