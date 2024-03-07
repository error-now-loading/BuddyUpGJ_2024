using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private PlayerInput playerInput;
    private Rigidbody2D rb;

    public List<MushroomMinion> minions = new List<MushroomMinion>(); //Temp public for testing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }

    void Update()
    {
        MovePlayer();
        UpdateDestinationFollow();
    }

    private void UpdateDestinationFollow()
    {
        for (int i = 0; i < minions.Count; i++)
        {
            minions[i].SetDestination(transform.position + new Vector3(i * -0.5f - 0.5f, 0,0));
        }
    }

    void MovePlayer()
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
}
