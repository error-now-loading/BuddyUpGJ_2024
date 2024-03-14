using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
[RequireComponent(typeof(Grid))]
public class Room : MonoBehaviour
{
    [SerializeField] private Tilemap terrainMap = null;
    [SerializeField] private Tilemap grassDecalMap = null;
    [SerializeField] private Tilemap mossDecalMap = null;
    [SerializeField] private Tilemap leafDecalMap = null;
    [SerializeField] private Tilemap obstacleMap = null;
}