using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : SceneSingleton<Map>
{
    private enum TileType : int
    {
        Terrain = 0,
        Decoration = 1
    }

    private enum TerrainType : int
    {
        LongGrass = 0,
        Muddy = 1
    }

    [SerializeField] private int mapWidth = 10;
    [SerializeField] private int mapHeight = 10;
    [SerializeField] private float mapScale = 5f;
    [SerializeField] private int mapSmoothness = 5;
    [SerializeField] private Grid mapGrid = null;

    [Header("Terrain Tiles")]
    [Tooltip("Number of tiles in list must be equal to number of TerrainTypes, order must match order in enum")]
    [SerializeField] private List<Tile> terrainTiles = null;

    [SerializeField] private Tilemap terrainMap = null;
    [SerializeField] private Tilemap decalMap = null;



    protected override void Awake()
    {
        base.Awake();

        if (terrainTiles.Count != Enum.GetNames(typeof(TerrainType)).Length)
        {
            Debug.LogError("[Map Generation Error]: Number of Tiles in terrainTiles not equal to number of terrain types");
            return;
        }

        float[,] noiseMap = PerlinNoiseGenerator.GenerateNoiseMap(mapWidth, mapHeight, mapScale, mapSmoothness, Vector2.zero);
        float[,] normalizedMap = NormalizeBiome(noiseMap);

        GenerateTerrainMap(normalizedMap);
    }

    /// <summary>
    ///     Normalize noise map for biomes
    /// </summary>
    /// <param name="biomeMap">Noise map of the biome</param>
    /// <returns>Normalized biome map</returns>
    private float[,] NormalizeBiome(float[,] biomeMap)
    {
        int width = biomeMap.GetLength(0);
        int height = biomeMap.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (biomeMap[x, y] > 0.4f)
                {
                    biomeMap[x, y] = (int)TerrainType.LongGrass;
                }

                else
                {
                    biomeMap[x, y] = (int)TerrainType.Muddy;
                }
            }
        }

        return biomeMap;
    }

    /// <summary>
    ///     Generates Tilemap object containing terrain data from provided normalized noise map
    /// </summary>
    /// /// <param name="normalizedBiomeMap">Tilemap object representing terrain factoring in biomes</param>
    /// <returns>Tilemap object representing provided map</returns>
    private void GenerateTerrainMap(float[,] normalizedBiomeMap)
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                terrainMap.SetTile(new Vector3Int(x, y, 0),
                                   terrainTiles[(int)normalizedBiomeMap[x, y]]);
            }
        }
    }
}