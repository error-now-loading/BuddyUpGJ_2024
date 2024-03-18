using System.Collections.Generic;
using UnityEngine;

public class Map : SceneSingleton<Map>
{
    [Tooltip("ALL Rooms should be configured to be the same size, with the same tilemap layers")]
    [SerializeField] private RoomVariantsSO roomVariants = null;
    private List<Room> rooms = null;
    private List<Vector3> roomPositions = new List<Vector3>()
    {
        new Vector3(20, -10),
        new Vector3(-20, 10),
        new Vector3(20, 10),
        new Vector3(40, 0),
        new Vector3(0, 20),
        new Vector3(-20, -10),
        new Vector3(-40, 0),
        new Vector3(0, -20),
    };



    protected override void Awake()
    {
        base.Awake();

        rooms = new List<Room>();
        for (int i = 0; i < 8; i++)
        {
            rooms.Add(Instantiate(roomVariants.SelectRandom(), roomPositions[i], Quaternion.identity));
        }
    }
}