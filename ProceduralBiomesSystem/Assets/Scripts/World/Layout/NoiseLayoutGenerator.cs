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

    private Map<EBiome> BiomeMap;

    public override WorldLayout GenerateWorldLayout(WorldSettings settings)
    {
        int mapWidth = settings.WorldSize.x;
        int mapHeight = settings.WorldSize.y;

        Map<float> elevationMap = new Map<float>(Utils.GenerateNoiseMap(mapWidth, mapHeight, elevation));
        Map<float> erosionMap = new Map<float>(Utils.GenerateNoiseMap(mapWidth, mapHeight, erosion));
        Map<float> humidityMap = new Map<float>(Utils.GenerateNoiseMap(mapWidth, mapHeight, humidity));

        BiomeMap = GenerateBiomeMap(mapWidth, mapHeight);

        return new WorldLayout.LayoutBuilder()
            .WithElevationMap(elevationMap)
            .WithErosionMap(erosionMap)
            .WithHumidityMap(humidityMap)
            .WithBiomeMap(BiomeMap)
            .Build();
    }

    Map<EBiome> GenerateBiomeMap(int width, int height)
    {
        // Use other maps to determine biomes somehow
        return new Map<EBiome>(width, height);

        //Map<EBiome> biomeMap = new Map<EBiome>(width, height);

        //for (int y = 0; y < height; y++)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        EBiome selectedBiome = EBiome.None;

        //        foreach (var kvp in biomeThresholds)
        //        {

        //            if (noiseValue < kvp.threshold)
        //            {
        //                selectedBiome = kvp.biome;
        //                break;
        //            }
        //        }

        //        biomeMap[x, y] = selectedBiome;
        //    }
        //}

        //return biomeMap;
    }
}