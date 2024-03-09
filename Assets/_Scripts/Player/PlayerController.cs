using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using System;

[RequireComponent(typeof(NutrientHandler))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform[] minionFollowPoints;
    [SerializeField] private float castingDuration = 1f;
    [SerializeField] private float commandDuration = 1f;

    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private Interactable closestInteractable;

    public List<MushroomMinion> minions = new List<MushroomMinion>(); //Temp public for testing

    private bool isBusy;
    private Coroutine waitingTimerCoroutine;

    public bool isCommanding { private set; get; }  //For Anims
    public bool isCasting { private set; get; }     //For Anims
    public event Action onRepeatCommand;     //For Anim Repeat

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

    private void FixedUpdate()
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
        if (movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movement.x > 0)
        {
            transform.localScale = Vector3.one;
        }
    }
    private void UpdateDestinationFollow()
    {
        for (int i = 0; i < minions.Count; i++)
        {
            minions[i].SetDestination(minionFollowPoints[i].transform.position);
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
            BusyForSeconds(castingDuration);
        }
    }
    public void Command(Interactable interactable)
    {
        // TODO: ADD LOGIC FOR COMMAND MUSHROOM
        if (isCommanding)
        {
            onRepeatCommand?.Invoke();
        }
        isCommanding = true;

        Vector3 direction = interactable.transform.position - transform.position;
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x > 0)
        {
            transform.localScale = Vector3.one;
        }

        BusyForSeconds(commandDuration);
    }
    private void BusyForSeconds(float seconds)
    {
        isBusy = true;
        StopCoroutine("WaitingTimer");
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
}