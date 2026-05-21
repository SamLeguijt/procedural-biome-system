using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainAnalyzer_", menuName = "ScriptableObjects/Analyzation/New TerrainAnalyzer")]
public class TerrainAnalyzer : ScriptableObject
{
    public TerrainAnalysisData AnalyzeTerrain(WorldData data)
    {
        var heightMap = data.TerrainMap; 
        var slopeMap = GenerateSlopeMap(heightMap);
        var primaryBiomes = GetPrimaryBiomeMap(data.BiomeMap);
        var biomeWeightsMap = data.BiomeMap;

        return new TerrainAnalysisData
        (
            heightMap,
            slopeMap,
            primaryBiomes,
            biomeWeightsMap
        );
    }

    private Map<BiomeConfig> GetPrimaryBiomeMap(Map<BiomeWeights> weightsMap)
    {
        Map<BiomeConfig> result = new Map<BiomeConfig>(weightsMap.Width, weightsMap.Height);

        for (int y = 0; y < weightsMap.Height; y++)
        {
            for (int x = 0; x < weightsMap.Width; x++)
            {
                (BiomeConfig, float) primary = weightsMap[x, y].GetHighest();
                result[x, y] = primary.Item1;

            }
        }

        return result;
    }

    /// TODO: Put these in different class(?)
    public static Map<float> GenerateSlopeMap(Map<float> heightMap, float slopeScale = 1f)
    {
        int width = heightMap.Width;
        int height = heightMap.Height;

        Map<float> slopeMap = new Map<float>(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                slopeMap[x, y] = CalculateSlope(heightMap, x, y, slopeScale);
            }
        }

        return slopeMap;
    }

    private static float CalculateSlope(Map<float> heightMap, int x, int y, float slopeScale)
    {
        float left = SampleHeight(heightMap, x - 1, y);
        float right = SampleHeight(heightMap, x + 1, y);
        float down = SampleHeight(heightMap, x, y - 1);
        float up = SampleHeight(heightMap, x, y + 1);

        float dx = right - left;
        float dy = up - down;

        float slope = Mathf.Sqrt(dx * dx + dy * dy);

        return Mathf.Clamp01(slope * slopeScale);
    }

    private static float SampleHeight(Map<float> map, int x, int y)
    {
        int clampedX = Mathf.Clamp(x, 0, map.Width - 1);
        int clampedY = Mathf.Clamp(y, 0, map.Height - 1);

        return map[clampedX, clampedY];
    }
}
