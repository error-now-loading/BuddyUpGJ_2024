using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RoomVariantsSO : ScriptableObject
{
    [SerializeField] public List<Room> variants = null;
    private Room lastRoom = null;



    public Room SelectRandom()
    {
        Room selectedRoom = variants[Random.Range(0, variants.Count)];
        if (lastRoom == null)
        {
            lastRoom = selectedRoom;
        }

        else
        {
            while (lastRoom == selectedRoom)
            {
                selectedRoom = variants[Random.Range(0, variants.Count)];
            }
            lastRoom = selectedRoom;
        }

        return selectedRoom;
    }
}