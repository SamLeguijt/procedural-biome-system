using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[CreateAssetMenu(fileName = "WorldGenerator_", menuName = "ScriptableObjects/World/new WorldGenerator")]
public class BiomeWorldGenerator : AbstractWorldGenerator
{
    public enum TerrainColorMode
    {
        DominantThreshold,
        DominantStrengthGrayscale,
        DominantWithBlend,
        DominantScaled,
        ColorBlend
    }


    [Header("Dependencies")]
    [SerializeField] private BaseBiomeAssigner biomeAssigner;
    [SerializeField] private BaseBiomeTerrainGenerator biomeTerrainGenerator;

    [SerializeField] private Material terrainMaterial = null;

    [Header("Terrain visuals")]
    [SerializeField] private TerrainColorMode colorMode;
    [SerializeField, Range(0f, 1f)]
    private float biomeVisualizationThreshold = 0.6f;


    private WorldData recentWorldData = null;

    

    public override WorldData GenerateWorld(WorldLayout layout)
    {
        Map<float> baseHeightMap = layout.ElevationMap;
        Map<BiomeWeights> rawBiomeMap = biomeAssigner.GenerateBiomeMap(layout);
        Map<float> terrainMap = biomeTerrainGenerator.GenerateTerrainMap(baseHeightMap, rawBiomeMap);

        Mesh terrainMesh = MeshGenerator.CreateMesh(terrainMap);
        Color[] colorMap = BiomeToColorMap(rawBiomeMap, terrainMesh.vertices.Length);
            
        terrainMesh.colors = colorMap;

        recentWorldData = new WorldData(terrainMesh, terrainMaterial, terrainMap, rawBiomeMap);
        return recentWorldData;
    }

    private void OnValidate()
    {
        if (recentWorldData != null)
        {
            ApplyBiomeToColorMap();
        }
    }

    private void Populate()
    {

    }

    private void ApplyBiomeToColorMap()
    {
        if (recentWorldData != null)
        {
            Color[] colorMap = BiomeToColorMap(recentWorldData.BiomeMap, recentWorldData.Mesh.vertices.Length);
            recentWorldData.Mesh.colors = colorMap;
        }
    }

    private Color[] BiomeToColorMap(Map<BiomeWeights> biomeMap, int verticesCount)
    {
        Color[] colorMap = new Color[verticesCount];

        int width = biomeMap.Width;
        int height = biomeMap.Height;

        int index = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color finalColor = Color.white;

                if (biomeMap.Contains(x, y))
                {
                    finalColor = EvaluateColor(biomeMap[x, y]);
                }

                colorMap[index] = finalColor;
                index++;
            }
        }

        return colorMap;
    }

    private Color EvaluateColor(BiomeWeights weights)
    {
        switch (colorMode)
        {
            case TerrainColorMode.DominantThreshold:
                return GetDominantThreshold(weights);

            case TerrainColorMode.DominantStrengthGrayscale:
                return GetDominantStrengthGrayscale(weights);

            case TerrainColorMode.DominantWithBlend:
                return GetDominantWithBlend(weights);

            case TerrainColorMode.DominantScaled:
                return GetDominantScaled(weights);

            case TerrainColorMode.ColorBlend:
                return weights.ToColor();

            default:
                return Color.magenta;
        }
    }

    private Color GetDominantThreshold(BiomeWeights weights)
    {
        float maxWeight = 0f;
        Color dominantColor = Color.white;

        foreach (var kvp in weights.ConfigWeights)
        {
            if (kvp.Value > maxWeight)
            {
                maxWeight = kvp.Value;
                dominantColor = kvp.Key.debugColor;
            }
        }

        return maxWeight >= biomeVisualizationThreshold
            ? dominantColor
            : Color.white;
    }

    private Color GetDominantStrengthGrayscale(BiomeWeights weights)
    {
        float maxWeight = 0f;

        foreach (var kvp in weights.ConfigWeights)
        {
            if (kvp.Value > maxWeight)
                maxWeight = kvp.Value;
        }

        Color color = Color.Lerp(Color.white, Color.black, maxWeight);

        return maxWeight >= biomeVisualizationThreshold
    ? color
    : Color.white;

    }

    private Color GetDominantWithBlend(BiomeWeights weights)
    {
        float maxWeight = 0f;
        float totalWeight = 0f;
        Color dominantColor = Color.white;

        foreach (var kvp in weights.ConfigWeights)
        {
            totalWeight += kvp.Value;

            if (kvp.Value > maxWeight)
            {
                maxWeight = kvp.Value;
                dominantColor = kvp.Key.debugColor;
            }
        }

        float purity = maxWeight / Mathf.Max(totalWeight, 0.0001f);
        float intensity = Mathf.Pow(purity, 1.8f);

        return Color.Lerp(Color.white, dominantColor, intensity);
    }

    private Color GetDominantScaled(BiomeWeights weights)
    {
        float maxWeight = 0f;
        Color dominantColor = Color.white;

        foreach (var kvp in weights.ConfigWeights)
        {
            if (kvp.Value > maxWeight)
            {
                maxWeight = kvp.Value;
                dominantColor = kvp.Key.debugColor;
            }
        }

        float intensity = Mathf.Pow(maxWeight, 0.5f);
        return Color.Lerp(Color.white, dominantColor, intensity);
    }
}
