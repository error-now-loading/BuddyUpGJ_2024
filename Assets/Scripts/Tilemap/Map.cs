using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.AI.Navigation;

public class Map : SceneSingleton<Map>
{
    private enum TileType : int
    {
        Terrain = 0,
        Decal = 1
    }

    private enum TerrainType : int
    {
        LongGrass = 0,
        Muddy = 1
    }

    private enum DecalType : int
    {
        Grass = 0,
        Moss = 1,
        Stone = 2,
        FallenLeaves = 3,
        NoDecal = 4
    }

    private enum ObstacleType : int
    {
        Stones = 0,
        NoObstacle = 1
    }



    [SerializeField] private int mapWidth = 10;
    [SerializeField] private int mapHeight = 10;
    [SerializeField] private float mapScale = 5f;
    [SerializeField] private int mapSmoothness = 5;
    [SerializeField] private NavMeshSurface navMeshSurface = null;

    [Header("Terrain Tiles")]
    [Tooltip("Order must match order in enum")]
    [SerializeField] private List<Tile> terrainTiles = null;
    [SerializeField] private Tilemap terrainMap = null;

    [Header("Decal Tiles")]
    [SerializeField] private TileVariantsSO grassVariants = null;
    [SerializeField] private TileVariantsSO mossVariants = null;
    [SerializeField] private TileVariantsSO stoneVariants = null;
    [SerializeField] private TileVariantsSO leafVariants = null;
    [SerializeField] private List<Tilemap> decalMaps = null;

    [Header("Obstacle Tiles")]
    [SerializeField] private Tilemap obstacleMap = null;
    [SerializeField] private TileVariantsSO stoneObstacleVariants = null;



    protected override void Awake()
    {
        base.Awake();

        float[,] noiseMap = PerlinNoiseGenerator.GenerateNoiseMap(mapWidth, mapHeight, mapScale, mapSmoothness, Vector2.zero);

        GenerateTerrainMap(NormalizeBiome(noiseMap));

        navMeshSurface.BuildNavMesh();
    }

    /// <summary>
    ///     Normalize noise map for biomes
    /// </summary>
    /// <param name="noiseMap">Noise map used for the biome</param>
    /// <returns>Normalized biome map</returns>
    private float[,] NormalizeBiome(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (noiseMap[x, y] > 0.4f)
                {
                    noiseMap[x, y] = (int)TerrainType.LongGrass;
                }

                else
                {
                    noiseMap[x, y] = (int)TerrainType.Muddy;
                }
            }
        }

        return noiseMap;
    }

    /// <summary>
    ///     Populated Terrain Tilemap object with terrain data from provided normalized noise map
    /// </summary>
    /// /// <param name="normalizedBiomeMap">Tilemap object representing terrain factoring in biomes</param>
    private void GenerateTerrainMap(float[,] normalizedBiomeMap)
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                terrainMap.SetTile(new Vector3Int(x, y, 0),
                                   terrainTiles[(int)normalizedBiomeMap[x, y]]);

                // If grass placed, determine whether to place grass or moss
                if ((int)normalizedBiomeMap[x, y] == (int)TerrainType.LongGrass)
                {
                    float randVal = UnityEngine.Random.Range(0f, 1f);

                    if (randVal > 0.5f)
                    {
                        decalMaps[(int)DecalType.Grass].SetTile(new Vector3Int(x, y, 0),
                                                                grassVariants.SelectRandom());
                    }

                    else if (randVal < 0.1f)
                    {
                        decalMaps[(int)DecalType.Moss].SetTile(new Vector3Int(x, y, 0),
                                                               mossVariants.SelectRandom());
                    }
                }

                // Determine whether to place leaf decal
                if (UnityEngine.Random.Range(0f, 1f) > 0.4f)
                {
                    decalMaps[(int)DecalType.FallenLeaves].SetTile(new Vector3Int(x, y, 0),
                                                                   leafVariants.SelectRandom());
                }

                // Determine whether to place stone decals
                if (UnityEngine.Random.Range(0f, 1f) > 0.7f)
                {
                    // Collidable or non-collidable stone?
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
                    {
                        decalMaps[(int)DecalType.Stone].SetTile(new Vector3Int(x, y, 0),
                                                                stoneVariants.SelectRandom());
                    }
                    
                    else
                    {
                        obstacleMap.SetTile(new Vector3Int(x, y, 1),
                                            stoneObstacleVariants.SelectRandom());
                    }
                }
            }
        }
    }
}