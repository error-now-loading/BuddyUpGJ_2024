using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[RequireComponent(typeof(NutrientHandler))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private Interactable closestInteractable;

    public List<MushroomMinion> minions = new List<MushroomMinion>(); //Temp public for testing
    [SerializeField] private Transform[] minionFollowPoints;

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
        MovePlayer();
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
        // TODO: ADD LOGIC FOR SPENDING NUTRIENTS WHEN SPELL IS CAST
    }
}