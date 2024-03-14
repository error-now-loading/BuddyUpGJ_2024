using System.Collections.Generic;
using UnityEngine;

public class Map : SceneSingleton<Map>
{
    [Tooltip("ALL Rooms should be configured to be the same size, with the same tilemap layers")]
    [SerializeField] private List<Room> roomVariants = null;
}