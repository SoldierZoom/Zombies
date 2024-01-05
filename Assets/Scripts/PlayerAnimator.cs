using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    [SerializeField] private Player player;
    private Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void Update() {
        animator.SetBool("isWalking",player.IsWalking);
        animator.SetBool("isSprinting",player.IsSprinting);
        animator.SetBool("isJumping",player.IsJumping);
        animator.SetBool("isAttacking",player.IsAttacking);
        animator.SetBool("rightHandWeapon",player.RightHandWeapon);
        animator.SetBool("leftHandWeapon",player.LeftHandWeapon);
        animator.SetBool("isOneHanded",player.IsOneHanded);
    }
}
