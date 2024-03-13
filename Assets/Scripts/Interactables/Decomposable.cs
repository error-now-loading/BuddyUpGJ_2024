using UnityEngine;

public class Decomposable : Interactable
{
    [SerializeField] private float decomposableHP = 100f;
    [SerializeField] private NutrientBall nutrientPrefab;
    public void GetHit(float damage)
    {
        decomposableHP -= damage;
        if (decomposableHP < 0 && !isFinished)
        {
            FinishTask();
            Instantiate(nutrientPrefab, transform.position, Quaternion.identity);
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
        GetHit(minion.GetDecomposeDamage());
    }
}
