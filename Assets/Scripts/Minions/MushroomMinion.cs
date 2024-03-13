using System;
using System.Collections;
using UnityEngine;

public class MushroomMinion : MonoBehaviour
{
    [SerializeField] private MushroomTypeSO mushroomType;
    [SerializeField] private float health = 100f;
    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private float attackDuration = 1f;
    [SerializeField] private float getDuration = 1f;

    private Rigidbody2D rb;
    private PlayerController assignedPlayer;

    private bool standingAlone = true;
    private Vector2 destination;
    private Interactable interactableTarget;
    private MinionSpot interactableSpot;

    private bool isBusy;
    private Coroutine waitingTimerCoroutine;

    public bool isDed { private set; get; }         //For Anims
    public bool isGet { private set; get; }         //For Anims
    public bool isCarrying { private set; get; }    //For Anims
    public event Action onAttack;                   //For Anims TODO attack logic

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health *= mushroomType.maxHpMultiplier;
    }

    void Update()
    {
        if (interactableSpot != null)
        {
            SetDestination(interactableSpot.transform.position);
        }
        if (!isBusy && !standingAlone && !isDed && !isCarrying)
        {
            GoToDestination();
        }
        if (isCarrying)
        {
            Work();
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
            if (interactableTarget != null)
            {
                BeginWork(interactableTarget);
            }
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
        isGet = true;
        BusyForSeconds(getDuration);
        assignedPlayer = FindObjectOfType<PlayerController>();
        assignedPlayer.MinionTroopJoin(this);
    }
    public void GetHit(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            isDed = true;
            rb.velocity = Vector3.zero;
            interactableSpot.occupied = false; 
            interactableSpot = null;
        }
    }
    public void SetDestination(Vector2 destination)
    {
        this.destination = destination;
    }
    public void SetTargetAndSpot(Interactable interactable, MinionSpot spot)
    {
        interactableTarget = interactable;
        interactableSpot = spot;
        assignedPlayer.MinionTroopRemove(this);
    }
    public MushroomTypeSO GetMushroomTypeSO()
    {
        return mushroomType;
    }
    public float GetAttackDamage()
    {
        return mushroomType.attackPerSecond * attackDuration;
    }
    public float GetDecomposeDamage()
    {
        return mushroomType.decomposePerSecond * attackDuration;
    }
    private void BeginWork(Interactable interactableTarget)
    {
        switch (interactableTarget.GetInteractableType())
        {
            case MushroomJobs.Attack:
            case MushroomJobs.Decompose:
                onAttack?.Invoke();
                transform.localScale = interactableSpot.transform.localScale;
                BusyForSeconds(attackDuration);
                break;
            case MushroomJobs.Carry:
                isCarrying = true;
                transform.position = interactableSpot.transform.position;
                transform.localScale = interactableSpot.transform.localScale;
                transform.parent = interactableSpot.transform;
                break;
            case MushroomJobs.Error:
                Debug.LogError("No Job type assigned to Interactable");
                break;
        };
    }
    private void Work()
    {
        if (interactableTarget.isFinished)
        {
            standingAlone = true;
            interactableSpot = null;
            interactableTarget = null;
        }
        else
        {
            interactableTarget.InteractMinion(this);
        }
    }
    private void BusyForSeconds(float seconds)
    {
        isBusy = true;
        if (waitingTimerCoroutine != null)
        {
            StopCoroutine(waitingTimerCoroutine);
        }
        waitingTimerCoroutine = StartCoroutine(WaitingTimer(seconds));
    }
    IEnumerator WaitingTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (!isGet && interactableTarget != null)
        {
            Work();
        }
        isBusy = false;
        isGet = false;
        waitingTimerCoroutine = null;
    }
}
