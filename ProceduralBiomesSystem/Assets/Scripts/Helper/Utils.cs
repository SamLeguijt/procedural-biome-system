using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float GetPerlinNoiseValue(Vector2 input, float frequency, float amplitude, Vector2 offset)
    {
        float x = (input.x * frequency + offset.x);
        float y = (input.y * frequency + offset.y);

        return Mathf.PerlinNoise(x, y) * amplitude;
    }

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        if (scale <= 0)
            scale = 0.0001f;

        System.Random random = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            float xOffset = random.Next(-100000, 100000) + offset.x;
            float yOffset = random.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(xOffset, yOffset);
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2;
        float halfHeight = mapHeight / 2;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1f;
                float frequency = 1f;
                float noiseHeight = 0;

                for (int o = 0; o < octaves; o++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[o].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[o].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                float normalisedHeight = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                noiseMap[x, y] = normalisedHeight;
            }
        }

        return noiseMap;
    }
}
