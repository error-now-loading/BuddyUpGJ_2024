using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RoomVariantsSO : ScriptableObject
{
    [SerializeField] public List<Room> variants = null;



    public Room SelectRandom()
    {
        return variants[Random.Range(0, variants.Count - 1)];
    }
}