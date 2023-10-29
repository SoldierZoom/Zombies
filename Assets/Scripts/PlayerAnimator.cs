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
        animator.SetBool("isFalling",player.IsFalling);
        if (player.IsJumping) {
            animator.Play("Jumping",-1,0f);
            Debug.Log("test");
        }
    }
}
