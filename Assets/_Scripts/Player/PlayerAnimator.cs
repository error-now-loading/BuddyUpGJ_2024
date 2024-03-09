using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        animator.SetFloat("playerSpeed",rb.velocity.magnitude);
        animator.SetBool("isCasting",player.isCasting);
        animator.SetBool("isCommanding",player.isCommanding);
    }
}
