using UnityEngine;

[CreateAssetMenu()]
public class MushroomTypeSO : ScriptableObject
{
    public MushroomTypes type;
    public MushroomMinion minionPrefab;
    public float attackPerSecond = 1;
    public float carryPerSecond = 1;
    public float decomposePerSecond = 1;
    public float maxHpMultiplier = 1;
}
public enum MushroomTypes
{
    Troopy,
    Bulky,
    Angy,
    Ghosty
}
public enum MushroomJobs
{
    Attack,
    Decompose,
    Carry,
    Error
}