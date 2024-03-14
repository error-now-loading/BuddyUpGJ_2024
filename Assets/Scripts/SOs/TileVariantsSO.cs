using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu()]
public class TileVariantsSO : ScriptableObject
{
    [SerializeField] public List<Tile> variants = null;



    public Tile SelectRandom()
    {
        return variants[Random.Range(0, variants.Count - 1)];
    }
}