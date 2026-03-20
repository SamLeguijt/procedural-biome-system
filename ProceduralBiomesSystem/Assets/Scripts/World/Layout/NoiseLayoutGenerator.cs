using System;
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
    [Header("Noise Settings")]
    [SerializeField] private NoiseSettings elevation; 
    [SerializeField] private NoiseSettings erosion; 
    [SerializeField] private NoiseSettings humidity; 

    public List<BiomeThreshold> biomeThresholds;

    private int recentWidth = 0; 
    private int recentHeight = 0;

    private void OnValidate()
    {
        if (recentWidth == 0 || recentHeight == 0)
            return;

        WorldLayout layout = GenerateWorldLayout(recentWidth, recentHeight); 
        OnLayoutChanged?.Invoke(layout);
    }
    public override WorldLayout GenerateWorldLayout(WorldSettings settings)
    {
        int mapWidth = settings.WorldSize.x;
        int mapHeight = settings.WorldSize.y;

        return GenerateWorldLayout(mapWidth, mapHeight);
    }

    public WorldLayout GenerateWorldLayout(int width, int height)
    {
        int mapWidth = width;
        int mapHeight = height;

        recentWidth = mapWidth;
        recentHeight = mapHeight;

        Map<float> elevationMap = new Map<float>(Utils.GenerateNoiseMap(mapWidth, mapHeight, elevation));
        Map<float> erosionMap = new Map<float>(Utils.GenerateNoiseMap(mapWidth, mapHeight, erosion));
        Map<float> humidityMap = new Map<float>(Utils.GenerateNoiseMap(mapWidth, mapHeight, humidity));
        Map<EBiome> biomeMap = GenerateBiomeMap(elevationMap, erosionMap, humidityMap);

        return new WorldLayout.LayoutBuilder()
            .WithElevationMap(elevationMap)
            .WithErosionMap(erosionMap)
            .WithHumidityMap(humidityMap)
            .WithBiomeMap(biomeMap)
            .Build();
    }

    private Map<EBiome> GenerateBiomeMap(Map<float> elevationMap, Map<float> erosionMap, Map<float> humidityMap)
    {
        int width = elevationMap.Width;
        int height = elevationMap.Height;

        Map<EBiome> biomeMap = new Map<EBiome> (width, height);


        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float elevationValue = elevationMap[x, y];
                float erosionValue = erosionMap[x, y];
                float humidityValue = humidityMap[x, y];

                EBiome chosenBiome = EBiome.None; 
                
                // TODO: Fix naive selection, magic numbers, architecture.
                if (elevationValue < 0.5f)
                {
                    if (humidityValue < 0.5f)
                        chosenBiome = EBiome.Desert;
                    else
                        chosenBiome = EBiome.Plains;
                }
                else
                {
                    if (humidityValue < 0.5f)
                        chosenBiome = EBiome.Volcanic;
                    else
                        chosenBiome = EBiome.Mountains;
                }

                biomeMap[x, y] = chosenBiome;
            }
        }


        return biomeMap;
    }
}