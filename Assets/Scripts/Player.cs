using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float movSpeed = 7f;
    [SerializeField] private float rotateSpeed = 12f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }
    private void HandleMovement() {
        //creates a movement vector from player's input
        Vector3 movDir = Vector3.zero;
        if (Input.GetKey("w")) {
            movDir += transform.forward;
        }if (Input.GetKey("a")) {
            movDir -= transform.right;
        }if(Input.GetKey("s")) {
            movDir -= transform.forward;
        }if(Input.GetKey("d")) {
            movDir += transform.right;
        }
        //normalises vector so player moves at the same speed diagonally
        movDir = movDir.normalized;
        //translates input vector to 3D space
        //Vector3 movDir = new Vector3(inputVector.x,0f,inputVector.y);

        //updates player's pos  direction they're facing
        transform.position += movDir * Time.deltaTime * movSpeed;
        //transform.forward = Vector3.Slerp(transform.forward,movDir,Time.deltaTime*rotateSpeed);


    }
}
