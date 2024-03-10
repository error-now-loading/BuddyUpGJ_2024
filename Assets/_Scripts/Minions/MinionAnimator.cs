using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAnimator : MonoBehaviour
{
    [SerializeField] private MushroomMinion minion;
    [SerializeField] private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        minion.onAttack += Minion_onAttack;
    }

    private void Minion_onAttack()
    {
        animator.SetTrigger("onAttack");
    }

    void Update()
    {
        animator.SetFloat("speed", rb.velocity.magnitude);
        animator.SetBool("isCarrying", minion.isCarrying);
        animator.SetBool("isDed", minion.isDed);
    }
}
