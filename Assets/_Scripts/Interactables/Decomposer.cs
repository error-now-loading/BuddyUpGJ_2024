using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NutrientHandler))]
public class Decomposer : Interactable
{
    private NutrientHandler handler = null;
    private List<Decomposable> decomposables = null;
    private bool isDecomposing = false;

    private void Awake()
    {
        handler = GetComponent<NutrientHandler>();
        decomposables = new List<Decomposable>();
    }

    private void FixedUpdate()
    {
        if (!isDecomposing && decomposables.Count > 0)
        {
            StartCoroutine(Decompose());
        }
    }

    public void AddDecomposable(Decomposable argValue)
    {
        if (!decomposables.Contains(argValue))
        {
            decomposables.Add(argValue);
        }
    }

    private IEnumerator Decompose()
    {
        isDecomposing = true;
        Debug.Log("DECOMPOSING");

        Decomposable currentDecomposable = decomposables[0];
        currentDecomposable.BeginDecomposing();
        yield return new WaitForSeconds(currentDecomposable.timeToDecompose);

        handler.AddNutrients(currentDecomposable.nutrientValue);
        decomposables.Remove(currentDecomposable);
        // Replace with object pool destroy call?
        Destroy(currentDecomposable.gameObject);

        isDecomposing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Decomposable decomposable;
        if (collision.gameObject.TryGetComponent(out decomposable))
        {
            AddDecomposable(decomposable);
        }
    }
    protected override void Interact()
    {
        NutrientHandler playerNutrients = playerReference.gameObject.GetComponent<NutrientHandler>();
        handler.TransferNutrients(playerNutrients);
        playerNutrients.PlayerNutrientRefillEvent();
    }
}