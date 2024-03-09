using UnityEngine;

[CreateAssetMenu()]
public class MushroomTypeSO : ScriptableObject
{
    public MushroomTypes type;
    public MushroomMinion minionPrefab;
    public float attackEfficiency = 1;
    public float transportEfficiency = 1;
    public float decomposeEfficiency = 1;
    public float MaxHpMultiplier = 1;
}
public enum MushroomTypes
{
    Troopy,
    Bulky,
    Angy
}