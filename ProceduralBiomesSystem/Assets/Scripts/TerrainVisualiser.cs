using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainVisualiser : MonoBehaviour
{
    private enum TerrainColorMode
    {
        DominantThreshold,
        DominantStrengthGrayscale,
        DominantWithBlend,
        DominantScaled,
        ColorBlend
    }

    [Header("Terrain visuals")]
    [SerializeField] private TerrainColorMode colorMode;
    [SerializeField, Range(0f, 1f)] private float biomeVisualizationThreshold = 0.6f;


    WorldData worldData = null;

    private void OnValidate()
    {
        if (worldData != null)
        {
            ApplyBiomeToColorMap();
        }
    }

    public void SetWorldData(WorldData data)
    {
        worldData = data;
        ApplyBiomeToColorMap();
    }

    private void ApplyBiomeToColorMap()
    {
        if (worldData != null)
        {
            Color[] colorMap = BiomeToColorMap(worldData.BiomeMap, worldData.Mesh.vertices.Length);
            worldData.Mesh.colors = colorMap;
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
