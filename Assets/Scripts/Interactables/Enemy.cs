using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Interactable
{
    [SerializeField] private float enemyHP = 100f;
    [SerializeField] private bool scavenger = false;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float visionRadius = 5f;
    [SerializeField] private float actionRadius = 1f;
    [SerializeField] private float attackDamagePerSecond = 1f;
    [SerializeField] private float eatDamagePerSecond = 1f;
    [SerializeField] private Decomposable corpsePrefab;

    [SerializeField] private float attackDuration = 1f;
    [SerializeField] private float fleeDuration = 1f;
    [SerializeField] private float eatDuration = 1f;
    [SerializeField] private float deathDuration = 1f;

    private PlayerController playerInRange;
    private List<MushroomMinion> minionsInRange = new List<MushroomMinion>();
    private List<MushroomMinion> attackingMinionsInRange = new List<MushroomMinion>();
    private List<NutrientBall> nutrientsInRange = new List<NutrientBall>();
    private List<Decomposable> decomposablesInRange = new List<Decomposable>();
    
    private Rigidbody2D rb;

    private bool aggroed;
    private bool isBusy;
    private Coroutine waitingTimerCoroutine;

    public bool isDed { private set; get; }         //For Anims
    public bool isEating { private set; get; }      //For Anims
    public event Action onAttack;                   //For Anims
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (!isBusy)
        {
            isEating = false; //Animator bug, needs to be here instead of WaitingTimer
            Vector2 moveDir = Vector2.zero;
            CheckSurroundings();
            if (scavenger && !aggroed)
            {
                if (decomposablesInRange.Count > 0 || nutrientsInRange.Count > 0)
                {
                    Interactable closestEatable = GetClosestEatable();
                    if (Vector2.Distance(closestEatable.transform.position, transform.position) < actionRadius)
                    {
                        // Eat food
                        isEating = true;
                        closestEatable.InteractEnemy(this);
                        BusyForSeconds(eatDuration);
                    }
                    else
                    {
                        // Go for food
                        moveDir = (closestEatable.transform.position - transform.position).normalized;
                    }
                }
                else if (minionsInRange.Count > 0 || playerInRange != null)
                {
                    //Flee
                    moveDir = (transform.position - GetClosestFearable()).normalized;
                    BusyForSeconds(fleeDuration);
                }
            }
            else
            {
                if (Vector2.Distance(transform.position, transform.position) < actionRadius)
                {
                    //Atack meanies
                }
                else
                {
                    // Go for meanies >:( moveDir
                }
            }
            //Pathing if notargets && isbusy moveDir
            Move(moveDir);
        }
    }

    private Interactable GetClosestEatable() // these functions irk me a bit, but they work xD
    {
        Interactable closestInteractable = null;
        if (decomposablesInRange.Count > 0 && nutrientsInRange.Count > 0)
        {
            Interactable closestDecomposable = decomposablesInRange
                .OrderBy(decomposable => Vector3.Distance(decomposable.transform.position, transform.position))
                .FirstOrDefault();
            Interactable closestNutrient = nutrientsInRange
                .OrderBy(nutrient => Vector3.Distance(nutrient.transform.position, transform.position))
                .FirstOrDefault();
            closestInteractable = Vector3.Distance(closestDecomposable.gameObject.transform.position, transform.position) < Vector3.Distance(closestNutrient.gameObject.transform.position, transform.position) ? closestDecomposable : closestNutrient;
        }
        else if (decomposablesInRange.Count > 0)
        {
            closestInteractable = decomposablesInRange
                .OrderBy(decomposable => Vector3.Distance(decomposable.transform.position, transform.position))
                .FirstOrDefault();
        }
        else
        {
            closestInteractable = nutrientsInRange
                .OrderBy(nutrient => Vector3.Distance(nutrient.transform.position, transform.position))
                .FirstOrDefault();
        }

        return closestInteractable;
    }

    private void Move(Vector2 moveDir)
    {
        rb.velocity = moveDir * moveSpeed;
        TurnMeTowards(moveDir);
    }
    private Vector3 GetClosestFearable() // these functions irk me a bit, but they work xD
    {
        Vector3 fleeFromPosition = Vector3.zero;
        if (minionsInRange.Count > 0 && playerInRange != null)
        {
            Vector3 closestMinion = minionsInRange
                .OrderBy(minion => Vector3.Distance(minion.transform.position, transform.position))
                .FirstOrDefault().transform.position;
            fleeFromPosition = Vector3.Distance(closestMinion, transform.position) < Vector3.Distance(playerInRange.transform.position, transform.position) ? closestMinion : playerInRange.transform.position;
        }
        else if (playerInRange)
        {
            fleeFromPosition = playerInRange.transform.position;
        }
        else
        {
            fleeFromPosition = minionsInRange
                .OrderBy(minion => Vector3.Distance(minion.transform.position, transform.position))
                .FirstOrDefault().transform.position;
        }
        return fleeFromPosition;
    }

    private void CheckSurroundings()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, visionRadius);
        playerInRange = null;
        minionsInRange = new List<MushroomMinion>();
        attackingMinionsInRange = new List<MushroomMinion>();
        nutrientsInRange = new List<NutrientBall>();
        decomposablesInRange = new List<Decomposable>();
        foreach (Collider2D collider in colliders)
        {
            PlayerController player = collider.GetComponent<PlayerController>();
            MushroomMinion minion = collider.GetComponent<MushroomMinion>();
            NutrientBall nutrient = collider.GetComponent<NutrientBall>();
            Decomposable decomposable = collider.GetComponent<Decomposable>();
            if (player != null)
            {
                playerInRange = player;
            }
            if (minion != null && !minionsInRange.Contains(minion))
            {
                minionsInRange.Add(minion);
            }
            else if (nutrient != null && !nutrientsInRange.Contains(nutrient))
            {
                nutrientsInRange.Add(nutrient);
            }
            else if (decomposable != null && !decomposablesInRange.Contains(decomposable))
            {
                decomposablesInRange.Add(decomposable);
            }

        }
        foreach (MinionSpot minionSpot in minionSpots)
        {
            if (minionSpot.occupied) {
                attackingMinionsInRange.Add(minionSpot.minion);
            }
        }
    }
    private MushroomMinion GetPriorityMinion(List<MushroomMinion> mushroomMinions)
    {
        List<MushroomMinion> highestPriorityMinions = new List<MushroomMinion>();

        float lowestHealth = mushroomMinions.Min(minion => minion.GetHp());
        highestPriorityMinions = mushroomMinions.Where(minion => minion.GetHp() <= lowestHealth).ToList();

        MushroomTypes highestPriority = highestPriorityMinions.Max(minion => minion.GetMushroomTypeSO().type);
        highestPriorityMinions = highestPriorityMinions.Where(minion => minion.GetMushroomTypeSO().type == highestPriority).ToList();

        MushroomMinion targetMinion = highestPriorityMinions
            .OrderBy(minion => Vector3.Distance(minion.transform.position, transform.position))
            .FirstOrDefault();
        return targetMinion;
    }

    public void GetHit(float damage)
    {
        enemyHP -= damage;
        aggroed = true;
        if (enemyHP < 0 && !isDed)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        isDed = true;
        yield return new WaitForSeconds(deathDuration);
        FinishTask();
        Instantiate(corpsePrefab, transform.position, Quaternion.identity);
    }

    protected override void Interact()
    {
        MushroomMinion minion = playerReference.TryToCommandMinionTo(this);
        if (minion != null)
        {
            TryAssignSpotTo(minion);
        }
    }
    public override void InteractMinion(MushroomMinion minion)
    {
        GetHit(minion.GetAttackDamage());
    }
    private void TurnMeTowards(Vector2 direction)
    {
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x > 0)
        {
            transform.localScale = Vector3.one;
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
        isBusy = false;
        waitingTimerCoroutine = null;
    }
    public float GetAttackDamage()
    {
        return attackDamagePerSecond * attackDuration;
    }
    public float GetEatDamage()
    {
        return eatDamagePerSecond * eatDuration;
    }
}