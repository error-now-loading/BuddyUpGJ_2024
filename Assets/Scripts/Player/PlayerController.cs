using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using System;

[RequireComponent(typeof(NutrientHandler))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float health = 100f;
    [SerializeField] private Transform[] minionFollowPoints;
    [SerializeField] private float castingDuration = 1f;
    [SerializeField] private float commandDuration = 1f;

    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private Interactable closestInteractable;

    private List<MushroomMinion> minionTroops = new List<MushroomMinion>();
    [SerializeField] private MushroomTypeSO selectedMushroomType;   //Serialize for now testing
    [SerializeField] private SpellTypeSO selectedSpellType;         //Serialize for now testing
    private bool isBusy;
    private Coroutine waitingTimerCoroutine;

    public bool isCommanding { private set; get; }  //For Anims
    public bool isCasting { private set; get; }     //For Anims
    public bool isDed { private set; get; }         //For Anims
    public event Action onRepeatCommand;            //For Anim Repeat

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerInput = new PlayerInput();
        playerInput.PlayerOverworld.Enable();

        playerInput.PlayerOverworld.Interact.performed += OnInteract;
        playerInput.PlayerOverworld.CastSpell.performed += OnSpellCast;
    }

    private void OnDisable()
    {
        playerInput.PlayerOverworld.Interact.performed -= OnInteract;
        playerInput.PlayerOverworld.CastSpell.performed -= OnSpellCast;
        playerInput.Disable();
    }

    private void Update()
    {
        if (!isBusy)
        {
            MovePlayer();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        UpdateDestinationFollow();
    }

    private void MovePlayer()
    {
        Vector2 movement = playerInput.PlayerOverworld.Movement.ReadValue<Vector2>();
        rb.velocity = movement * moveSpeed;
        TurnMeTowards(movement);
    }
    private void UpdateDestinationFollow()
    {
        for (int i = 0; i < minionTroops.Count; i++)
        {
            minionTroops[i].SetDestination(minionFollowPoints[i].transform.position);
        }
    }
    private void OnInteract(InputAction.CallbackContext ctx)
    {
        closestInteractable?.NearInteraction();
    }
    public void SetClosestNearInteractable(Interactable interactable)
    {
        closestInteractable = interactable;
    }
    private void OnSpellCast(InputAction.CallbackContext ctx)
    {
        if (!isBusy)
        {
            // TODO: ADD LOGIC FOR SPENDING NUTRIENTS WHEN SPELL IS CAST
            isCasting = true;
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = cursorPosition - transform.position;
            TurnMeTowards(direction);
            Instantiate(selectedSpellType.spellPrefab, new Vector3(cursorPosition.x,cursorPosition.y, 0), Quaternion.identity);
            BusyForSeconds(castingDuration);
        }
    }
    public MushroomMinion TryToCommandMinionTo(Interactable interactable)
    {
        if (isCommanding)
        {
            onRepeatCommand?.Invoke();
        }
        isCommanding = true;
        Vector3 direction = interactable.transform.position - transform.position;
        TurnMeTowards(direction);
        BusyForSeconds(commandDuration);
        return PickMinion();
    }

    private MushroomMinion PickMinion()
    {
        foreach (MushroomMinion minion in minionTroops)
        {
            if(minion.GetMushroomTypeSO() == selectedMushroomType) return minion;
        }
        return null;
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
        isCasting = false;
        isCommanding = false;
        waitingTimerCoroutine = null;
    }

    public void MinionTroopJoin(MushroomMinion mushroomMinion)
    {
        minionTroops.Add(mushroomMinion);
    }
    public void MinionTroopRemove(MushroomMinion mushroomMinion)
    {
        minionTroops.Remove(mushroomMinion);
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
    public void GetHit(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            isDed = true;
            rb.velocity = Vector3.zero;
        }
    }
}