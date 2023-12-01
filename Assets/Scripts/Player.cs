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
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private Transform groundCheck;
    private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpHeight=10f;
    Vector3 v;
    bool isGrounded;
    //interaction
    [SerializeField] private LayerMask interactLayer;
    //picking up weapon
    [SerializeField] private Transform playerRightHand;
    //animation states
    bool isWalking, isSprinting, isJumping;

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

        //executes jump ability if player is on the ground
        if(isGrounded && Input.GetButtonDown("Jump")) {
            //sets velocity to initial velocity required to reach desired jump height
            v.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;
        }

        //adds gravity to player
        v.y += gravity * Time.deltaTime;
        characterController.Move(v * Time.deltaTime);
    }

    private void HandleInteractions() {
        float playerHeight = 3f;
        //creating 3D vector for direction player is looking/moving
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movDir = transform.right * x + transform.forward * z;


        //checking if object in the interact layer is within 2 units of the player in the direction they're looking
        if(Physics.CapsuleCast(transform.position + Vector3.up * playerHeight,transform.position,.5f,movDir,out RaycastHit hitInfo,2f,interactLayer)) {
            //Physics.CapsuleCast(transform.position+Vector3.up*playerHeight, transform.position, .5f, movDir, out RaycastHit hitInfo, 2f, interactLayer)
            //Physics.Raycast(transform.position+Vector3.up*playerHeight,movDir,out RaycastHit hitInfo,2f,interactLayer)
            if(hitInfo.transform.CompareTag("Weapon")) {
                hitInfo.transform.position = playerRightHand.position;
                hitInfo.transform.parent = playerRightHand;
                hitInfo.transform.localRotation = Quaternion.Euler(new Vector3(15f,-15f));
                hitInfo.transform.localPosition = new Vector3(-0.0696f,-0.0002f,-0.0032f);
            }
            Debug.Log("Interact!");
        }
    }

    public bool IsWalking { get { return isWalking; } }
    public bool IsSprinting { get { return isSprinting; } }
    public bool IsJumping { get { return isJumping; } }
}
