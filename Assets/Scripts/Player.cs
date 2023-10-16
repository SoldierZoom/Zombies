using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour {

    [SerializeField] private CharacterController characterController;
    [SerializeField] private float movSpeed = 7f;
    private float sprintMultiplier;

    [SerializeField] private float gravityMag = -9.8f;
    [SerializeField] private Transform groundCheck;
    float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpStrength=10f;

    Vector3 v;
    bool isGrounded;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        HandleMovement();
    }
    private void HandleMovement() {
        //creates sphere at bottom of player to check if they have reached the ground
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundLayer);
        //sets velocity to -2 when player reaches the ground
        if(isGrounded && v.y<0) {
            v.y = -2f;
        }
        //reseting sprint multiplier
        sprintMultiplier = 1f;
        //creates movement vector relative to the player's position based on their input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movDir = transform.right * x + transform.forward * z;
        //adds sprint multiplier if sprintkey pressed
        if(Input.GetKey(KeyCode.LeftShift)) {
            sprintMultiplier = 3f;
        }
        //provides charcter controller with movement vector for player
        characterController.Move(movDir*movSpeed*sprintMultiplier*Time.deltaTime);

        if(isGrounded && Input.GetButtonDown("Jump")) {
            v.y = Mathf.Sqrt(jumpStrength * -2f * gravityMag);
        }



        //adds gravity to player
        v.y += gravityMag * Time.deltaTime;
        characterController.Move(v * Time.deltaTime);
    }
}
