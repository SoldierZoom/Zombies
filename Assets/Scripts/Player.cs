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
    [SerializeField] private Transform playerRightHand, playerLeftHand, floatingGun, playerCamera;
    private MeleeWeaponScriptObj rightHandSO, leftHandSO;
    private RangedWeaponSO rangedWeaponSO;
    private bool meleeEquipped;
    //animation state bools
    private bool isWalking, isSprinting, isJumping, isAttacking, rightHandWeapon, leftHandWeapon, rangedWeapon, isOneHanded;
    [SerializeField] private Animator animator;
    //shooting
    [SerializeField] private LayerMask hittableLayer;
    float counter=0;

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
        //gets movement vector for direction player is moving
        Vector3 movDir = getMovDir();
        //making isWalking true when player is moving
        if(movDir != Vector3.zero) {
            isWalking = true;
        }
        //adds sprint multiplier and activates isSprinting if sprintkey pressed and attack anim isn't playing
        if(Input.GetKey(KeyCode.LeftShift)&&!AttackAnimPlaying()) {
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
        Vector3 movDir = getMovDir();

        weaponRef = null;
        //checking if object in the interact layer is within 2 units of the player in the direction they're looking
        if(Physics.CapsuleCast(transform.position + Vector3.up * playerHeight,transform.position,.5f,movDir,out RaycastHit hitInfo,2f,interactLayer)) {
            weaponRef = hitInfo.transform;
            //checking if weapon is melee through "Melee" tag
            if(weaponRef.CompareTag("Melee")) {
                //checks if player has no weapon in right hand
                if(!rightHandWeapon) {
                    //finding scriptable object for weapon
                    rightHandSO = weaponList.FindMeleeSO(weaponRef.name);
                    isOneHanded = rightHandSO.IsOneHanded();
                    IntitialiseWeapon(weaponRef,playerRightHand);
                    rightHandWeapon = true;
                    Debug.Log("Right Weapon in hand = " + weaponRef.name);

                    //sets leftHand bool to true if weapon is 2 handed
                    if(!isOneHanded) {
                        leftHandWeapon = true;
                        Debug.Log("Left Weapon in hand = " + weaponRef.name);
                    } if(rangedWeapon) {
                        ToggleWeapon(floatingGun,0,false);
                    }
                    //checks if player only has weapon in right hand and picked object isn't the weapon in their right hand
                } else if(rightHandWeapon && !leftHandWeapon && weaponRef.parent != playerRightHand) {
                    leftHandSO = weaponList.FindMeleeSO(weaponRef.name);
                    //makes sure weapon isn't two handed
                    if (leftHandSO.IsOneHanded()) {
                        IntitialiseWeapon(weaponRef,playerLeftHand);
                        leftHandWeapon = true;
                        Debug.Log("Left Weapon in hand = " + weaponRef.name);
                    } else {
                        Debug.Log("Hands are full");
                    }if(rangedWeapon) {
                        ToggleWeapon(floatingGun,0,false);
                        ToggleWeapon(playerRightHand,5,true);
                    }
                }
            } if(weaponRef.CompareTag("Ranged")) {
                if(!rangedWeapon) {
                    Debug.Log("Picked up ranged Weapon");
                    rangedWeaponSO = weaponList.FindRangedSO(weaponRef.name);
                    rangedWeaponSO.IntialiseAmmo();
                    IntitialiseWeapon(weaponRef,floatingGun,Quaternion.identity,Vector3.zero);
                    rangedWeapon = true;
                    if(IsMeleeEquipped()) {
                        if(rightHandWeapon) {
                            ToggleWeapon(playerRightHand,5,false);
                        }
                        if(leftHandWeapon && isOneHanded) {
                            ToggleWeapon(playerLeftHand,5,false);
                        }
                    }
                }
            }
        }
    }
    
    private void HandleControls() {
        //resets isAttacking each update so anim doesn't play continuous
        isAttacking = false;
        //drops the weapon currently in the player's hand
        if(!AttackAnimPlaying()) {
            if(Input.GetKeyDown(KeyCode.Q)) {
                if(IsMeleeEquipped()) {
                    if(leftHandWeapon) {
                        leftHandWeapon = false;
                        if(isOneHanded) {
                            DropWeapon(playerLeftHand,5);
                            Debug.Log("Left Weapon dropped");
                        } else {
                            rightHandWeapon = false;
                            DropWeapon(playerRightHand,5);
                            Debug.Log("Right Weapon dropped");
                        }
                    } else if(rightHandWeapon) {
                        rightHandWeapon = false;
                        DropWeapon(playerRightHand,5);
                        Debug.Log("Right Weapon dropped");
                    }
                } else {
                    if(rangedWeapon) {
                        rangedWeapon = false;
                        DropWeapon(floatingGun,0);
                        Debug.Log("Gun dropped");
                    }
                }
            } else if(Input.GetKeyDown(KeyCode.Mouse0) && (rightHandWeapon || rangedWeapon)) {
                isAttacking = true;
                //Debug.Log("Attack!");
                if(!IsMeleeEquipped()&&rangedWeapon&&counter<=0) {
                    Debug.Log("Shot fired");
                    rangedWeaponSO.Shoot(floatingGun.position,playerCamera.forward,hittableLayer);
                    counter = rangedWeaponSO.GetShootDelay()*Time.deltaTime;
                } else if(counter>0) {
                    counter-=Time.deltaTime;
                }
            } else if((Input.GetKeyDown(KeyCode.Alpha1) && !IsMeleeEquipped() && rightHandWeapon) || (Input.GetKeyDown(KeyCode.Alpha2)) && (!rightHandWeapon || IsMeleeEquipped())) {
                if(rightHandWeapon) {
                    ToggleWeapon(playerRightHand,5);
                }
                if(leftHandWeapon && isOneHanded) {
                    ToggleWeapon(playerLeftHand,5);
                }
                if(rangedWeapon) {
                    ToggleWeapon(floatingGun,0);
                }
            } else if(Input.GetKeyDown(KeyCode.R)&&rangedWeapon&&!IsMeleeEquipped()) {
                rangedWeaponSO.Reload();
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
        if(rightHandWeapon) {
            return playerRightHand.GetChild(5).gameObject.activeSelf;
        } else {
            return false;
        }
    }
    public bool IsOneHanded { get { return isOneHanded; } }
    //general functions
    private void IntitialiseWeapon(Transform weapon, Transform parent, Quaternion rotation, Vector3 pos) {
        weapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon.parent = parent;
        weapon.localRotation = rotation;
        weapon.localPosition = pos;
    }
    private void IntitialiseWeapon(Transform weapon, Transform parent) {
        IntitialiseWeapon(weapon, parent,Quaternion.Euler(new Vector3(15f,-15f)),new Vector3(-0.0696f,-0.0002f,-0.0032f));
    }
    private void DropWeapon(Transform parent,int index) {
        Transform weapon = parent.GetChild(index);
        weapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.parent = null;
    }
    private void ToggleWeapon(Transform parent,int index, bool state) {
        GameObject weapon = parent.GetChild(index).gameObject;
        weapon.SetActive(state);
    }
    private void ToggleWeapon(Transform parent,int index) {
        GameObject weapon = parent.GetChild(index).gameObject;
        weapon.SetActive(!weapon.activeSelf);
    }
    //returns movement vector relative to the player's position based on their input
    private Vector3 getMovDir() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        return (transform.right * x + transform.forward * z);
    }

    private bool AttackAnimPlaying() {
        if(animator.GetCurrentAnimatorStateInfo(1).IsName("Melee Attack A1")||animator.GetCurrentAnimatorStateInfo(2).IsName("Melee Attack A1")) {
            return true;
        } else {
            return false;
        }
    }
}
