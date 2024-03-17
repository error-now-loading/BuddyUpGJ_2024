using UnityEngine;

[CreateAssetMenu()]
public class SpellTypeSO : ScriptableObject
{
    public SpellTypes type;
    public GameObject spellPrefab;
    public int manaCost;
}
public enum SpellTypes
{
    BuffAttackDamage,
    BuffCarrySpeed,
    BuffDecomposeDamage,
    SummonDummy,
    SummonTroopy,
    SummonBulky,
    SummonAngy,
    SummonGhosty,
    WinGame,
    NullBuff,
}