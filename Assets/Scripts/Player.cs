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
    private Transform weaponRef;
    [SerializeField] private Transform playerRightHand, playerLeftHand, floatingGun;
    private MeleeWeaponScriptObj rightHandSO, leftHandSO;
    private RangedWeaponSO rangedWeaponSO;
    private bool meleeEquipped;
    //animation state bools
    private bool isWalking, isSprinting, isJumping, isAttacking, rightHandWeapon, leftHandWeapon, rangedWeapon, isOneHanded;
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
        //adds sprint multiplier and activates isSprinting if sprintkey pressed and attack anim isn't playing
        if(Input.GetKey(KeyCode.LeftShift)&&(animator.GetCurrentAnimatorStateInfo(1).IsName("Melee Attack A1")==false||animator.GetCurrentAnimatorStateInfo(2).IsName("Melee Attack A1") == false)) {
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
            //checking if weapon is melee through "Melee" tag
            if(weaponRef.CompareTag("Melee")) {
                //checks if player has no weapon in right hand
                if(rightHandWeapon == false) {
                    //finding scriptable object for weapon
                    rightHandSO = weaponList.FindSO(weaponRef.name);
                    isOneHanded = rightHandSO.IsOneHanded();
                    IntitialiseWeapon(weaponRef,playerRightHand);
                    rightHandWeapon = true;
                    Debug.Log("Right Weapon in hand = " + weaponRef.name);

                    //sets leftHand bool to true if weapon is 2 handed
                    if(isOneHanded==false) {
                        leftHandWeapon = true;
                        Debug.Log("Left Weapon in hand = " + weaponRef.name);
                    }
                    //checks if player only has weapon in right hand and picked object isn't the weapon in their right hand
                } else if(rightHandWeapon == true && leftHandWeapon==false && weaponRef.parent != playerRightHand) {
                    leftHandSO = weaponList.FindSO(weaponRef.name);
                    //makes sure weapon isn't two handed
                    if (leftHandSO.IsOneHanded()==true) {
                        IntitialiseWeapon(weaponRef,playerLeftHand);
                        leftHandWeapon = true;
                        Debug.Log("Left Weapon in hand = " + weaponRef.name);
                    } else {
                        Debug.Log("Hands are full");
                    }
                }
            } else if(weaponRef.CompareTag("Ranged")) {

            }
        }
    }
    
    private void HandleControls() {
        //resets isAttacking each update so anim doesn't play continuous
        isAttacking = false;
        if(Input.GetKeyDown(KeyCode.Q)) {
            if(leftHandWeapon==true) {
                leftHandWeapon = false;
                if(isOneHanded==true) {
                    leftHandSO = null;
                    DropWeapon(playerLeftHand,5);
                    Debug.Log("Left Weapon dropped");
                } else {
                    rightHandWeapon = false;
                    rightHandSO=null;
                    DropWeapon(playerRightHand,5);
                    Debug.Log("Right Weapon dropped");
                }
            } else if (rightHandWeapon==true) {
                rightHandWeapon = false;
                rightHandSO = null;
                DropWeapon(playerRightHand,5);
                Debug.Log("Right Weapon dropped");
            }
        } else if(rightHandWeapon==true && Input.GetKeyDown(KeyCode.Mouse0)) {
            isAttacking = true;
            Debug.Log("Attack!");
        } else if(Input.GetKeyDown(KeyCode.Alpha1) && (animator.GetCurrentAnimatorStateInfo(1).IsName("Melee Attack A1") == false || animator.GetCurrentAnimatorStateInfo(2).IsName("Melee Attack A1") == false)) {
            if(rightHandWeapon==true) {
                ToggleWeapon(playerRightHand,5);
            } if(leftHandWeapon==true&&isOneHanded==true) {
                ToggleWeapon(playerLeftHand,5);
            }
        }

    }


    //animation bool return functions
    public bool IsWalking { get { return isWalking; } }
    public bool IsSprinting { get { return isSprinting; } }
    public bool IsJumping { get { return isJumping; } }
    public bool IsAttacking { get { return isAttacking; } }
    public bool RightHandWeapon { get { return rightHandWeapon; } }
    public bool LeftHandWeapon { get { return leftHandWeapon; } }
    public bool IsMeleeEquipped() { 
        if(rightHandWeapon == true) {
            return playerRightHand.GetChild(5).gameObject.activeSelf;
        } else {
            return false;
        }
    }
    public bool IsOneHanded { get { return isOneHanded; } }
    //general functions
    private void IntitialiseWeapon(Transform weapon, Transform parent) {
        weapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon.parent = parent;
        weapon.localRotation = Quaternion.Euler(new Vector3(15f,-15f));
        weapon.localPosition = new Vector3(-0.0696f,-0.0002f,-0.0032f);
    }
    private void DropWeapon(Transform parent,int index) {
        Transform weapon = parent.GetChild(index);
        weapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.parent = null;
    }
    private void ToggleWeapon(Transform parent,int index) {
        GameObject weapon = parent.GetChild(index).gameObject;
        weapon.SetActive(!weapon.activeSelf);
    }

}
