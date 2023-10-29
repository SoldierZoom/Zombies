using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour {

    [SerializeField] private CharacterController characterController;
    //movement
    [SerializeField] private float movSpeed = 7f;
    private float sprintMultiplier;
    //gravity
    [SerializeField] private float gravityMag = -9.8f;
    [SerializeField] private Transform groundCheck;
    private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpStrength=10f;
    Vector3 v;
    bool isGrounded;
    //interaction
    [SerializeField] private LayerMask interactLayer;
    //animation states
    bool isWalking, isSprinting, isJumping, isFalling;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        HandleMovement();
        if(Input.GetKey(KeyCode.E)) {
            HandleInteractions();
        }
    }
    private void HandleMovement() {
        //reseting all animation states
        isWalking = false;
        isSprinting = false;
        isJumping = false;
        isFalling = false;
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
        //making isWalking true when player is moving
        if(movDir != Vector3.zero) {
            isWalking = true;
        }
        //adds sprint multiplier and activates isSprinting if sprintkey pressed
        if(Input.GetKey(KeyCode.LeftShift)) {
            sprintMultiplier = 3f;
            isSprinting = true;
        }
        //provides charcter controller with movement vector for player 
        characterController.Move(movDir.normalized*movSpeed*sprintMultiplier*Time.deltaTime);

        if(isGrounded && Input.GetButtonDown("Jump")) {
            v.y = Mathf.Sqrt(jumpStrength * -2f * gravityMag);
            isJumping = true;
        }

        //adds gravity to player
        v.y += gravityMag * Time.deltaTime;
        characterController.Move(v * Time.deltaTime);

        if(!isGrounded&&v.y<0) {
            isFalling=true;
        }
    }

    private void HandleInteractions() {
        //creating 3D vector for direction player is looking/moving
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movDir = transform.right * x + transform.forward * z;

        //checking if object in the interact layer is within 2 units of the player in the direction they're looking
        if(Physics.Raycast(transform.position,movDir,out RaycastHit hitInfo,2f,interactLayer)) {
            Debug.Log("Interact!");
        }
    }

    public bool IsWalking { get { return isWalking; } }
    public bool IsSprinting { get { return isSprinting; } }
    public bool IsJumping { get { return isJumping; } }
    public bool IsFalling { get { return isFalling; } }
}
