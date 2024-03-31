using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        player.onRepeatCommand += Player_onRepeatCommand;
    }

    private void Player_onRepeatCommand()
    {
        animator.SetTrigger("commandRepeat");
    }

    void Update()
    {
        animator.SetFloat("playerSpeed",rb.velocity.magnitude);
        animator.SetBool("isCasting",player.isCasting);
        animator.SetBool("isFailingCasting", player.isFailingCasting);
        animator.SetBool("isDed", player.isDed);
        animator.SetBool("isCommanding",player.isCommanding);
    }
}