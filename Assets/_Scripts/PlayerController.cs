using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float interactDistance = 0.75f;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private NutrientHandler nutrientHandler = null;

    private PlayerInput playerInput;
    private Rigidbody2D rb;

    public event Action<int> OnNutrientValueChange;



    private void Start()
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
    }

    private void MovePlayer()
    {
        Vector2 movement = playerInput.PlayerOverworld.Movement.ReadValue<Vector2>();
        rb.velocity = movement * moveSpeed;
        if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        RaycastHit2D hit;
        if (spriteRenderer.flipX == true)
        {
            hit = Physics2D.Raycast(gameObject.transform.position, gameObject.transform.right, -interactDistance);
        }
        else
        {
            hit = Physics2D.Raycast(gameObject.transform.position, gameObject.transform.right, interactDistance);
        }

        if (hit.collider != null)
        {
            // TODO: Replace with layer enum check?
            if (hit.collider.gameObject.tag == "Decomposer")
            {
                NutrientHandler otherHandler;
                if (hit.collider.gameObject.TryGetComponent(out otherHandler))
                {
                    otherHandler.TransferNutrients(nutrientHandler);

                    OnNutrientValueChange?.Invoke(nutrientHandler.nutrients);
                }
            }
        }
    }

    private void OnSpellCast(InputAction.CallbackContext ctx)
    {
        // TODO: ADD LOGIC FOR SPENDING NUTRIENTS WHEN SPELL IS CAST
    }
}
