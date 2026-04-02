using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    public MapVisualizer mapVisualiser;

    public float scale;

    [Button]
    void NoiseMap()
    {
        Map<float> noiseMap = new Map<float>(GenerateSimpleNoise(200, 200, scale));
        mapVisualiser.DrawFloatMap(noiseMap, Color.white);
    }

    [Button]
    void FalloffMap()
    {
        Map<float> map = new Map<float>(GenerateFalloffMap(200, 200));

        mapVisualiser.DrawFloatMap(map, Color.white);
    }

    [Button]
    void Combined()
    {
        float[,] noise = GenerateSimpleNoise(200, 200, scale);
        float[,] falloff = GenerateFalloffMap(200, 200);

        Map<float> map = new Map<float>(ApplyFalloff(noise, falloff));

        mapVisualiser.DrawFloatMap(map, Color.white);
    }

    public static float[,] GenerateSimpleNoise(int width, int height, float scale)
    {
        float[,] map = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float sampleX = x / scale;
                float sampleY = y / scale;

                float noise = Mathf.PerlinNoise(sampleX, sampleY);
                map[x, y] = noise;
            }
        }

        return map;
    }

    public static float[,] GenerateFalloffMap(int width, int height)
    {
        float[,] map = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float nx = x / (float)(width - 1) * 2f - 1f;
                float ny = y / (float)(height - 1) * 2f - 1f;

                float distance = Mathf.Max(Mathf.Abs(nx), Mathf.Abs(ny));

                map[x, y] = distance;
            }
        }

        return map;
    }

    public static float[,] ApplyFalloff(float[,] noise, float[,] falloff)
    {
        int width = noise.GetLength(0);
        int height = noise.GetLength(1);

        float[,] result = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float n = noise[x, y];
                float f = falloff[x, y];
                f = Mathf.Pow(f, 1f);

                float value = Mathf.Lerp(n, 0f, f);

                result[x, y] = value;
            }
        }

        return result;
    }
}
