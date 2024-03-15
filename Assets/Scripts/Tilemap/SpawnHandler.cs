using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnHandler : MonoBehaviour
{
    // TODO: Implement timed spawn
    [SerializeField] private int numEnemySpawners = 2;
    [SerializeField] private int numResourceSpawners = 2;
    [Space]
    [SerializeField] private EnemyVariantsSO enemyVariants = null;
    [SerializeField] private ResourceVariantsSO resourceVariants = null;
    [Space]
    [SerializeField] private Tilemap obstacleMap = null; // Spawners will use obstacleMap to determine valid spawn locations

    private List<MushroomMinion> enemiesForSpawners = null; // TODO: Replace with Enemy class
    private List<Decomposable> resourcesForSpawners = null;

    private List<Vector3> enemySpawnLocations = null;
    private List<Vector3> resourceSpawnLocations = null;
    private List<Vector3> availableLocations = null;
    


    private void Awake()
    {
        enemiesForSpawners = new List<MushroomMinion>();
        resourcesForSpawners = new List<Decomposable>();

        for (int i = 0; i < numEnemySpawners; i++)
        {
            enemiesForSpawners.Add(enemyVariants.SelectRandom());
        }

        for (int i = 0; i < numResourceSpawners; i++)
        {
            resourcesForSpawners.Add(resourceVariants.SelectRandom());
        }

        SelectTilesForSpawners();
        SpawnEntities();
    }

    private void SelectTilesForSpawners()
    {
        enemySpawnLocations = new List<Vector3>();
        resourceSpawnLocations = new List<Vector3>();
        availableLocations = new List<Vector3>();

        for (int i = obstacleMap.cellBounds.xMin + 1; i < obstacleMap.cellBounds.xMax; i++)
        {
            for (int j = obstacleMap.cellBounds.yMin; j < obstacleMap.cellBounds.yMax - 1; j++)
            {
                Vector3Int localPlace = new Vector3Int(i, j, 0);
                Vector3 place = obstacleMap.CellToWorld(localPlace);

                if (!obstacleMap.HasTile(localPlace))
                {
                    availableLocations.Add(place);
                }
            }
        }

        availableLocations.Shuffle();

        enemySpawnLocations.AddRange(availableLocations.GetRange(0, numEnemySpawners));
        resourceSpawnLocations.AddRange(availableLocations.GetRange(numEnemySpawners, numResourceSpawners));
        availableLocations.RemoveRange(0, numEnemySpawners + numResourceSpawners);
    }

    private void SpawnEntities()
    {
        // Spawn enemies
        for (int i = 0; i < numEnemySpawners; i++)
        {
            Instantiate(enemiesForSpawners[i], enemySpawnLocations[i], Quaternion.identity);
        }

        // Spawn resources
        for (int i = 0; i < numResourceSpawners; i++)
        {
            Instantiate(resourcesForSpawners[i], resourceSpawnLocations[i], Quaternion.identity);
        }
    }
}