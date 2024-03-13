using UnityEngine;

[CreateAssetMenu()]
public class SpellTypeSO : ScriptableObject
{
    public SpellTypes type;
    public GameObject spellPrefab;
    public float manaCost;
}
public enum SpellTypes
{
    BuffAttackSpeed,
    BuffCarrySpeed,
    BuffDecomposeSpeed,
    SummonDummy,
    SummonTroopy,
    SummonBulky,
    SummonAngy,
    SummonGhosty,
    WinGame
}