using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float movSpeed = 7f;
    //[SerializeField] private GameInput gameInput;

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
        Vector2 inputVector = Vector2.zero;
        if (Input.GetKey("w")) {
            inputVector.x = 1;
        }if (Input.GetKey("a")) {
            inputVector.y = 1;
        }if(Input.GetKey("s")) {
            inputVector.x = -1;
        }if(Input.GetKey("d")) {
            inputVector.y = -1;
        }
        inputVector = inputVector.normalized;
        Vector3 movDir = new Vector3(inputVector.x,0,inputVector.y);

       transform.position += movDir*Time.deltaTime*movSpeed;
    }
}
