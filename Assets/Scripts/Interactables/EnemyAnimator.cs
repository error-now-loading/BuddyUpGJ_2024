using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        enemy.onAttack += Minion_onAttack;
    }

    private void Minion_onAttack()
    {
        animator.SetTrigger("onAttack");
    }

    void LateUpdate()
    {
        animator.SetFloat("speed", rb.velocity.magnitude);
        animator.SetBool("isEating", enemy.isEating);
        animator.SetBool("isDed", enemy.isDed);
    }
}