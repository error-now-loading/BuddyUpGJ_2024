using UnityEngine;

public class MushroomMinion : MonoBehaviour
{
    [SerializeField] private MushroomTypeSO mushroomType;
    //[SerializeField] private float maxHP = 100f;
    [SerializeField] private float moveSpeed = 10f;

    private bool standing = false; //Should be true, testing
    private Vector2 destination;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!standing)
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
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    public void SetDestination(Vector2 destination)
    {
        this.destination = destination;
    }
}
