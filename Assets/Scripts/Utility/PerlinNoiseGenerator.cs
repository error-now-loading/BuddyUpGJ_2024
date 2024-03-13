using UnityEngine;

/// <summary>
///     Utility class for noise map generation
/// </summary>
public static class PerlinNoiseGenerator
{
    /// <summary>
    ///     Generates 2D Perlin Noise matrix
    /// </summary>
    /// <param name="width">Matrix width in pixels</param>
    /// <param name="height">Matrix height in pixels</param>
    /// <param name="scale">"Zoom" level in Perlin Noise, larger results in smaller groupings</param>
    /// <param name="octaves">"Smoothness" level in Perlin Noise</param>
    /// <param name="position">Point of generation</param>
    /// <returns>2D float[,] matrix</returns>
    public static float[,] GenerateNoiseMap(int width, int height, float scale, int octaves, Vector2 position)
    {
        float[,] noiseMap = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float sampleX = (float)x / width * scale;
                float sampleY = (float)y / width * scale;

                float noise = 0f;
                float frequency = 0f;
                for (int oct = 1; oct < octaves; oct *= 2)
                {
                    float rawPerlin = Mathf.PerlinNoise(oct * (sampleX + (position.x * scale)),
                                                        oct * (sampleY + (position.y * scale)));

                    frequency += 1f / oct;
                    noise += (1f / oct) * Mathf.Clamp(rawPerlin, 0f, 1f);
                }

                noise /= frequency;
                noiseMap[x, y] = noise;
            }
        }

        return noiseMap;
    }
}