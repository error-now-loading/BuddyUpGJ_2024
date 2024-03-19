using System.Collections.Generic;
using UnityEngine;

public abstract class VariantSO<T> : ScriptableObject where T : class
{
    [SerializeField] public List<T> variants = null;



    public virtual T SelectRandom()
    {
        return variants[Random.Range(0, variants.Count)];
    }
}