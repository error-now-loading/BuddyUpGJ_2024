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
    [SerializeField] private float deathDuration = 1f;

    [SerializeField] private AudioSource attackSource = null;
    [SerializeField] private AudioSource hurtSource = null;
    [SerializeField] private AudioSource minionSource = null;

    private Rigidbody2D rb;
    private PlayerController assignedPlayer;

    private bool standingAlone = true;
    private bool autoTask = true;
    private Vector2 destination;
    private Interactable interactableTarget;
    private MinionSpot interactableSpot;

    private bool isBusy;
    private Coroutine waitingTimerCoroutine;
    private static SpellTypes activeBuff = SpellTypes.NullBuff;
    private float activeBuffMultiplier = 1.5f;
    private float autoTaskTimer = 3f;

    public bool isDed { private set; get; }                     //For Anims
    public bool isGet { private set; get; }                     //For Anims
    public bool isCarrying { private set; get; }                //For Anims
    public event Action onAttack;                               //For Anims
    public static event Action onMinionCountChange;             //For UI
    public static int minionCount { private set; get; } = 0;    //For UI



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health *= mushroomType.maxHpMultiplier;
        minionCount++;
        onMinionCountChange?.Invoke();
    }

    private void FixedUpdate()
    {
        if (interactableSpot != null && interactableSpot.transform != null)
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
        if (!standingAlone && !isBusy && interactableTarget == null && assignedPlayer == null)
        {
            SetStandAlone(true);
        }
    }

    private void GoToDestination()
    {
        Vector2 currentPos = rb.position;
        Vector2 moveDir = (destination - currentPos).normalized;

        if (Vector2.Distance(currentPos, destination) > 0.1f)
        {
            rb.velocity = moveDir * moveSpeed;
            TurnMeTowards(moveDir);
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (autoTask && standingAlone && collision.gameObject.GetComponent<Interactable>())
        {
            if(!(mushroomType.type == MushroomTypes.Ghosty && collision.gameObject.GetComponent<Interactable>().GetInteractableType() == MushroomJobs.Carry))
            {
                standingAlone = false;
                autoTask = false;
                collision.gameObject.GetComponent<Interactable>().TryAssignSpotTo(this);
            }
        }
    }

    public void JoinPlayer()
    {
        SetStandAlone(false);
        isGet = true;
        AudioManager.instance.PlaySFX(minionSource, AudioManager.instance.minionGet);
        BusyForSeconds(getDuration);
        assignedPlayer = FindObjectOfType<PlayerController>();
        assignedPlayer.MinionTroopJoin(this);
    }
    public void GetHit(float damage)
    {
        health -= damage;

        if (mushroomType.type == MushroomTypes.Bulky)
        {
            AudioManager.instance.PlaySFX(hurtSource, AudioManager.instance.bulkyHurt);
        }

        else
        {
            AudioManager.instance.PlaySFX(hurtSource, AudioManager.instance.minionHurt);
        }

        if (health <= 0 && !isDed)
        {
            StartCoroutine(Die());
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
        if (assignedPlayer != null)
        {
            assignedPlayer.MinionTroopRemove(this);
        }
    }

    public MushroomTypeSO GetMushroomTypeSO()
    {
        return mushroomType;
    }

    public float GetAttackDamage()
    {
        return mushroomType.attackPerSecond * attackDuration * (activeBuff == SpellTypes.BuffAttackDamage ? activeBuffMultiplier : 1);
    }

    public float GetDecomposeDamage()
    {
        return mushroomType.decomposePerSecond * attackDuration * (activeBuff == SpellTypes.BuffDecomposeDamage ? activeBuffMultiplier : 1);
    }

    public float GetCarryPower()
    {
        return mushroomType.decomposePerSecond * Time.deltaTime * (activeBuff == SpellTypes.BuffCarrySpeed ? activeBuffMultiplier : 1);  
    }

    private void BeginWork(Interactable interactableTarget)
    {
        TurnMeTowards(interactableTarget.transform.position - transform.position);
        switch (interactableTarget.GetInteractableType())
        {
            case MushroomJobs.Attack:
                onAttack?.Invoke();
                BusyForSeconds(attackDuration);

                if (mushroomType.type == MushroomTypes.Bulky)
                {
                    AudioManager.instance.PlaySFX(attackSource, AudioManager.instance.bulkyAttack);
                }

                else if (mushroomType.type == MushroomTypes.Troopy)
                {
                    AudioManager.instance.PlaySFX(attackSource, AudioManager.instance.troopyAttack);
                }

                else if (mushroomType.type == MushroomTypes.Angy)
                {
                    AudioManager.instance.PlaySFX(attackSource, AudioManager.instance.angyAttack);
                }
                
                break;

            case MushroomJobs.Decompose:
                onAttack?.Invoke();
                BusyForSeconds(attackDuration);

                if (mushroomType.type == MushroomTypes.Bulky)
                {
                    AudioManager.instance.PlaySFX(attackSource, AudioManager.instance.bulkyAttack);
                }

                else if (mushroomType.type == MushroomTypes.Troopy)
                {
                    AudioManager.instance.PlaySFX(attackSource, AudioManager.instance.troopyAttack);
                }

                else if (mushroomType.type == MushroomTypes.Angy)
                {
                    AudioManager.instance.PlaySFX(attackSource, AudioManager.instance.angyAttack);
                }

                else if (mushroomType.type == MushroomTypes.Ghosty)
                {
                    AudioManager.instance.PlaySFX(attackSource, AudioManager.instance.minionAttack);
                }

                break;

            case MushroomJobs.Carry:
                isCarrying = true;
                transform.position = interactableSpot.transform.position;
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
            Release();
        }
        else
        {
            interactableTarget.InteractMinion(this);
        }
    }
    public void Release()
    {
        if(interactableSpot != null)
        {
            interactableSpot.occupied = false;
            interactableSpot.minion = null;
        }
        SetStandAlone(true);
        interactableSpot = null;
        interactableTarget = null;
        isCarrying = false;
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

    public float GetHp()
    {
        return health;
    }
    IEnumerator Die()
    {
        isDed = true;

        if (mushroomType.type == MushroomTypes.Bulky)
        {
            AudioManager.instance.PlaySFX(hurtSource, AudioManager.instance.bulkyDeath);
        }

        else
        {
            AudioManager.instance.PlaySFX(hurtSource, AudioManager.instance.minionDeath);
        }

        rb.velocity = Vector3.zero;
        minionCount--;
        onMinionCountChange?.Invoke();
        if (interactableSpot != null)
        {
            interactableSpot.occupied = false;
            interactableSpot.minion = null;
            interactableSpot = null;
        }
        yield return new WaitForSeconds(deathDuration);
        Destroy(gameObject);
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

    public static void SetActiveBuff(SpellTypes buffType)
    {
        activeBuff = buffType;
    }

    public void SetStandAlone(bool boolean)
    {
        if (boolean && !standingAlone)
        {
            standingAlone = true;
            Invoke("AutoTask", autoTaskTimer);
        }
        else
        {
            standingAlone = false;
            autoTask = false;
        }
    }

    private void AutoTask()
    {
        if (standingAlone)
        {
            autoTask = true;
        }
    }

    public static void ResetMinionCount()
    {
        minionCount = 0;
    }
    public static void RefreshMinionCount()
    {
        minionCount = FindObjectsOfType<MushroomMinion>().Length;
        onMinionCountChange?.Invoke();
    }

    public void CallBack()
    {
        if (!isDed)
        {
            isCarrying = false;
            if (waitingTimerCoroutine != null)
            {
                StopCoroutine(waitingTimerCoroutine);
                waitingTimerCoroutine = null;
            }
            if (interactableSpot != null)
            {
                interactableSpot.occupied = false;
                interactableSpot.minion = null;
                interactableSpot = null;
            }
            if (interactableTarget != null)
            {
                interactableTarget = null;
            }
            transform.parent = null;
            JoinPlayer();
        }
    }
}