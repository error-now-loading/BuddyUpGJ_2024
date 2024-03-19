using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnHandler : MonoBehaviour
{
    // TODO: Implement timed spawn
    [SerializeField] private int numEnemySpawners = 2;
    [SerializeField] private float enemySpawnInterval = 30f;
    [Space]
    [SerializeField] private int numResourceSpawners = 2;
    [SerializeField] private float resourceSpawnInterval = 30f;
    [Space]
    [SerializeField] private EnemyVariantsSO enemyVariants = null;
    [SerializeField] private ResourceVariantsSO resourceVariants = null;
    [Space]
    // Spawners use obstacleMap to determine valid spawn locations
    [SerializeField] private Tilemap obstacleMap = null; 

    private List<Enemy> spawnedEnemies = null;
    private List<Decomposable> spawnedResources = null;

    private List<Vector3> enemySpawnLocations = null;
    private List<Vector3> resourceSpawnLocations = null;
    private List<Vector3> availableLocations = null;

    private bool canSpawnEnemies = true;
    private bool canSpawnResources = true;



    private void Awake()
    {
        spawnedEnemies = new List<Enemy>();
        spawnedResources = new List<Decomposable>();

        enemySpawnLocations = new List<Vector3>();
        resourceSpawnLocations = new List<Vector3>();
        availableLocations = new List<Vector3>();

        SelectTilesForSpawners();
    }

    private void FixedUpdate()
    {
        if (canSpawnEnemies)
        {
            StartCoroutine(SpawnEnemies());
        }

        if (canSpawnResources)
        {
            StartCoroutine(SpawnResources());
        }
    }

    private void SelectTilesForSpawners()
    {
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

    private IEnumerator SpawnEnemies()
    {
        canSpawnEnemies = false;
        spawnedEnemies.RemoveAll(item => item == null);
        int numToSpawn = numEnemySpawners - spawnedEnemies.Count;

        if (numToSpawn > 0)
        {
            for (int i = 0; i < numToSpawn; i++)
            {
                Enemy newEnemy = Instantiate(enemyVariants.SelectRandom(), enemySpawnLocations[Random.Range(0, enemySpawnLocations.Count)], Quaternion.identity);
                spawnedEnemies.Add(newEnemy);
            }

            yield return new WaitForSeconds(enemySpawnInterval);
        }

        else
        {
            yield return new WaitForSeconds(enemySpawnInterval);
        }

        canSpawnEnemies = true;
    }

    private IEnumerator SpawnResources()
    {
        canSpawnResources = false;
        spawnedResources.RemoveAll(item => item == null);
        int numToSpawn = numResourceSpawners - spawnedResources.Count;

        if (numToSpawn > 0)
        {
            for (int i = 0; i < numToSpawn; i++)
            {
                Decomposable newDecomposable = Instantiate(resourceVariants.SelectRandom(), resourceSpawnLocations[Random.Range(0, resourceSpawnLocations.Count)], Quaternion.identity);
                spawnedResources.Add(newDecomposable);
            }

            yield return new WaitForSeconds(resourceSpawnInterval);
        }

        else
        {
            yield return new WaitForSeconds(resourceSpawnInterval);
        }

        canSpawnResources = true;
    }
}