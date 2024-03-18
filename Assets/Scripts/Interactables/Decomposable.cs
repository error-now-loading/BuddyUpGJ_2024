using UnityEngine;

public class Decomposable : Interactable
{
    [SerializeField] private float decomposableHP = 100f;
    [SerializeField] private float decomposableByEnemyHP = 100f;
    [SerializeField] private DecomposableMask mask;
    [SerializeField] private NutrientBall nutrientPrefab;
    private int maxDivisions;
    private int currentDivision;
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
            NutrientBall ball = Instantiate(nutrientPrefab, transform.position, Quaternion.identity);
            if (currentDivision > 0)
            {
                ball.ReduceNutrient(ball.nutrientValue / maxDivisions * currentDivision);
            }
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
                this.currentDivision++;
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
