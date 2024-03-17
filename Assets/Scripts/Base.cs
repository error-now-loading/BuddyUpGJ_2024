using UnityEngine;

[RequireComponent(typeof(NutrientHandler))]
public class Base : Interactable
{
    [SerializeField] private NutrientHandler nutrientHandler = null;
    private NutrientHandler playerNutrientHandler = null;



    protected override void Interact()
    {
        if (playerNutrientHandler == null)
        {
            playerNutrientHandler = playerReference.GetComponent<NutrientHandler>();
        }

        nutrientHandler.TransferNutrients(playerNutrientHandler);
    }
}