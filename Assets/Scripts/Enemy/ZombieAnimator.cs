using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimator : MonoBehaviour {
    [SerializeField] private Zombie zombie;
    private Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void Update() {
        animator.SetBool("isWalking",zombie.IsWalking);
        animator.SetBool("isSprinting",zombie.IsSprinting);
        animator.SetBool("isAttacking",zombie.IsAttacking);
    }
}
