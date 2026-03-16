using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public struct BiomeThreshold
{
    public EBiome biome;
    public float threshold;
}

[CreateAssetMenu(fileName = "LayoutGenerator_", menuName = "ScriptableObjects/World/new LayoutGenerator")]
public class NoiseLayoutGenerator : AbstractLayoutGenerator
{
    [Header("Map Settings")]
    [SerializeField] int width = 200;
    [SerializeField] int height = 200;

    [Header("Noise Settings")]
    [SerializeField] bool randomSeed = true;
    [SerializeField] int seed = 0;
    [SerializeField] float scale = 50;
    [SerializeField] int octaves = 4;
    [SerializeField] float persistance = 0.5f;
    [SerializeField] float lacunarity = 2f;
    [SerializeField] Vector2 offset;
    [SerializeField] public int heightMultiplier;

    public List<BiomeThreshold> biomeThresholds;

    Map<EBiome> BiomeMap;
    Texture2D debugTexture;

    public override WorldLayout GenerateWorldLayout(WorldSettings settings)
    {
        BiomeMap = Generate();
        return new WorldLayout(BiomeMap);
    }

    Map<EBiome> Generate()
    {
        if (randomSeed)
        {
            int randomSeed = Random.Range(-10000, 10000);
            seed = randomSeed;
        }
            var noiseMap = Utils.GenerateNoiseMap(
            width,
            height,
            seed,
            scale,
            octaves,
            persistance,
            lacunarity,
            offset
        );

        int mapWidth = noiseMap.GetLength(0);
        int mapHeight = noiseMap.GetLength(1);

        Map<EBiome> biomeMap = new Map<EBiome>(mapWidth, mapHeight);

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                EBiome selectedBiome = EBiome.None;

                foreach (var kvp in biomeThresholds)
                {
                    float noiseValue = noiseMap[x, y];

                    if (noiseValue < kvp.threshold)
                    {
                        selectedBiome = kvp.biome;
                        break;
                    }
                }

                biomeMap[x, y] = selectedBiome;
            }
        }

        return biomeMap;
    }
}