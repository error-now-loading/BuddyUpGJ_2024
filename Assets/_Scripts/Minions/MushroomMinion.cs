using System;
using UnityEngine;

public class MushroomMinion : MonoBehaviour
{
    [SerializeField] private MushroomTypeSO mushroomType;
    [SerializeField] private float health = 100f;
    [SerializeField] private float moveSpeed = 10f;

    private Rigidbody2D rb;

    private bool standingAlone = true;
    private Vector2 destination;

    public bool isDed { private set; get; }         //For Anims
    public bool isCarrying { private set; get; }    //For Anims
    public event Action onAttack;                   //For Anims TODO attack logic

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!standingAlone && !isDed)
        {
            GoToDestination();
        }
    }
    private void GoToDestination()
    {
        Vector2 currentPos = rb.position;
        Vector2 moveDir = (destination - currentPos).normalized;

        if (Vector2.Distance(currentPos, destination) > 0.1f)
        {
            rb.velocity = moveDir * moveSpeed;
            if (moveDir.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (moveDir.x > 0)
            {
                transform.localScale = Vector3.one;
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (standingAlone && collision.GetComponent<NearRangeTrigger>())
        {
            JoinPlayer();
        }
    }

    public void JoinPlayer()
    {
        standingAlone = false;
        FindObjectOfType<PlayerController>().MinionTroopJoin(this);
    }
    public void GetHit(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            isDed = true;
            rb.velocity = Vector3.zero;
        }
    }
    public void SetDestination(Vector2 destination)
    {
        this.destination = destination;
    }
}
