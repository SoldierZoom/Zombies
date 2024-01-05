using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
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
    [SerializeField] WeaponList weaponList;
    [SerializeField] private Transform playerRightHand, playerLeftHand;
    private Transform weaponRef;
    private MeleeWeaponScriptObj rightHandSO, leftHandSO;
    //private bool weaponInHand;
    //animation state bools
    private bool isWalking, isSprinting, isJumping, isAttacking, rightHandWeapon, leftHandWeapon, isOneHanded;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        HandleMovement();
        if(Input.GetKey(KeyCode.E)) {
            HandleInteractions();
        }
        HandleControls();
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
        if(Input.GetKey(KeyCode.LeftShift)&&animator.GetCurrentAnimatorStateInfo(0).IsName("Melee Attack A1")==false) {
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

        weaponRef = null;
        //checking if object in the interact layer is within 2 units of the player in the direction they're looking
        if(Physics.CapsuleCast(transform.position + Vector3.up * playerHeight,transform.position,.5f,movDir,out RaycastHit hitInfo,2f,interactLayer)) {
            weaponRef = hitInfo.transform;
            if(weaponRef.CompareTag("Melee")) {
                if(rightHandWeapon == false) {
                    rightHandSO = weaponList.FindSO(weaponRef.name);
                    isOneHanded = rightHandSO.IsOneHanded();

                    weaponRef.GetComponent<Rigidbody>().isKinematic = true;
                    weaponRef.parent = playerRightHand;
                    weaponRef.localRotation = Quaternion.Euler(new Vector3(15f,-15f));
                    weaponRef.localPosition = new Vector3(-0.0696f,-0.0002f,-0.0032f);
                    rightHandWeapon = true;
                    Debug.Log("Right Weapon in hand = " + weaponRef.name);

                    if(isOneHanded==false) {
                        leftHandWeapon = true;
                        Debug.Log("Left Weapon in hand = " + weaponRef.name);
                    }

                } else if(rightHandWeapon == true && leftHandWeapon==false) {
                    leftHandSO = weaponList.FindSO(weaponRef.name);
                    if (leftHandSO.IsOneHanded()==true) {
                        weaponRef.GetComponent<Rigidbody>().isKinematic = true;
                        weaponRef.parent = playerLeftHand;
                        weaponRef.localRotation = Quaternion.Euler(new Vector3(15f,-15f));
                        weaponRef.localPosition = new Vector3(-0.0696f,-0.0002f,-0.0032f);
                        leftHandWeapon = true;
                        Debug.Log("Left Weapon in hand = " + weaponRef.name);
                    } else {
                        Debug.Log("Hands are full");
                    }
                }
            }
        }
    }
    
    private void HandleControls() {
        isAttacking = false;
        if(Input.GetKeyDown(KeyCode.Q)) {
            if(leftHandWeapon==true) {
                leftHandWeapon = false;
                if(isOneHanded==true) {
                    leftHandSO = null;
                    weaponRef = playerLeftHand.transform.GetChild(5).transform;
                    weaponRef.GetComponent<Rigidbody>().isKinematic = false;
                    weaponRef.parent = null;
                    Debug.Log("Left Weapon dropped");
                } else {
                    rightHandWeapon = false;
                    rightHandSO=null;
                    weaponRef = playerRightHand.transform.GetChild(5).transform;
                    weaponRef.GetComponent<Rigidbody>().isKinematic = false;
                    weaponRef.parent = null;
                    Debug.Log("Right Weapon dropped");
                }
            } else if (rightHandWeapon==true) {
                rightHandWeapon = false;
                rightHandSO = null;
                weaponRef = playerRightHand.transform.GetChild(5).transform;
                weaponRef.GetComponent<Rigidbody>().isKinematic = false;
                weaponRef.parent = null;
                Debug.Log("Right Weapon dropped");
            }
        } else if(rightHandWeapon==true && Input.GetKeyDown(KeyCode.Mouse0)) {
            isAttacking = true;
            Debug.Log("Attack!");
        }

    }



    public bool IsWalking { get { return isWalking; } }
    public bool IsSprinting { get { return isSprinting; } }
    public bool IsJumping { get { return isJumping; } }
    public bool IsAttacking { get { return isAttacking; } }
    public bool RightHandWeapon { get { return rightHandWeapon; } }
    public bool LeftHandWeapon { get { return leftHandWeapon; } }
    public bool IsOneHanded { get { return isOneHanded; } }


}
