using UnityEngine;

public class Decomposable : Interactable
{
    [SerializeField] private float decomposableHP = 100f;
    [SerializeField] private int nutrientValue = 1;
    [SerializeField] private NutrientBall nutrientPrefab;
    public void GetHit(float damage)
    {
        decomposableHP -= damage;
        if (decomposableHP < 0)
        {
            //Instance prefab, assign values
            Debug.Log("nooo");
            Destroy(gameObject);
        }
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
        //Decompose GetHit();
    }
}
