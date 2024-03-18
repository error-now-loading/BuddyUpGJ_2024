using UnityEngine;

public class Decomposable : Interactable
{
    [SerializeField] private float decomposableHP = 100f;
    [SerializeField] private float decomposableByEnemyHP = 100f;
    [SerializeField] private DecomposableMask mask;
    [SerializeField] private NutrientBall nutrientPrefab;
    private int maxDivisions;
    private float maxByEnemyHp;
    private void Awake()
    {
        maxDivisions = mask.GetNumberOfDivisions();
        maxByEnemyHp = decomposableByEnemyHP;
    }
    public void GetHit(float damage)
    {
        decomposableHP -= damage;
        if (decomposableHP < 0 && !isFinished)
        {
            FinishTask();
            Instantiate(nutrientPrefab, transform.position, Quaternion.identity);
        }
    }
    public void GetHitByBug(float damage)
    {
        decomposableByEnemyHP -= damage;
        if (decomposableByEnemyHP < 0 && !isFinished)
        {
            FinishTask();
        }
        else
        {
            float divisionSize = maxByEnemyHp / maxDivisions;
            int currentDivision = Mathf.FloorToInt((maxByEnemyHp - decomposableByEnemyHP) / divisionSize) + 1;
            int previousDivision = Mathf.FloorToInt((maxByEnemyHp - (decomposableByEnemyHP + damage)) / divisionSize) + 1;
            if (currentDivision != previousDivision)
            {
                mask.IncreaseDivision();
            }
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
    public override void InteractEnemy(Enemy enemy)
    {
        GetHitByBug(enemy.GetEatDamage());
    }
}
