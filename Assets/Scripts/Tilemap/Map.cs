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
        LongGrassDecal = 0,
        NoDecal = 1
    }



    [SerializeField] private int mapWidth = 10;
    [SerializeField] private int mapHeight = 10;
    [SerializeField] private float mapScale = 5f;
    [SerializeField] private int mapSmoothness = 5;
    [SerializeField] private NavMeshSurface navMeshSurface = null;

    [Header("Terrain Tiles")]
    [Tooltip("Number of tiles in list must be equal to number of TerrainTypes, order must match order in enum")]
    [SerializeField] private List<Tile> terrainTiles = null;
    [SerializeField] private Tilemap terrainMap = null;

    [Header("Decal Tiles")]
    [Tooltip("Number of tiles in list must be one less than number of DecalTypes, order must match order in enum")]
    [SerializeField] private List<Tile> decalTiles = null;
    [SerializeField] private Tilemap decalMap = null;



    protected override void Awake()
    {
        base.Awake();

        if (terrainTiles.Count != Enum.GetNames(typeof(TerrainType)).Length)
        {
            Debug.LogError("[Map Generation Error]: Number of Tiles in terrainTiles not equal to number of terrain types");
            return;
        }

        if (decalTiles.Count != Enum.GetNames(typeof(DecalType)).Length - 1)
        {
            Debug.LogError("[Map Generation Error]: Number of Decal Tiles in decalTiles not equal to number of decal types");
            return;
        }

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

                // If grass placed, determine whether or not to place grass decal as well
                if ((int)normalizedBiomeMap[x, y] == (int)TerrainType.LongGrass)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
                    {
                        decalMap.SetTile(new Vector3Int(x, y, 0),
                                         decalTiles[(int)DecalType.LongGrassDecal]);
    }
                }
            }
        }   
    }
}