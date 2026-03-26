using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "LayoutGenerator_", menuName = "ScriptableObjects/World/new LayoutGenerator")]
public class NoiseLayoutGenerator : AbstractLayoutGenerator
{
    [Header("Noise Settings")]
    [SerializeField] private NoiseSettings elevation; 
    [SerializeField] private NoiseSettings erosion; 
    [SerializeField] private NoiseSettings humidity; 
    [SerializeField] private NoiseSettings temperature; 

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

        Map<float> elevationMap = new Map<float>(NoiseGenerator.GenerateNoiseMap(mapWidth, mapHeight, elevation));
        Map<float> erosionMap = new Map<float>(NoiseGenerator.GenerateNoiseMap(mapWidth, mapHeight, erosion));
        Map<float> humidityMap = new Map<float>(NoiseGenerator.GenerateNoiseMap(mapWidth, mapHeight, humidity));
        Map<float> temperatureMap = new Map<float>(NoiseGenerator.GenerateNoiseMap(mapWidth, mapHeight, temperature));

        return new WorldLayout.LayoutBuilder()
            .WithElevationMap(elevationMap)
            .WithErosionMap(erosionMap)
            .WithHumidityMap(humidityMap)
            .WithTemperatureMap(temperatureMap)
            .Build();
    }
}