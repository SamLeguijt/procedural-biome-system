using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainAnalyzer_", menuName = "ScriptableObjects/Analyzation/New TerrainAnalyzer")]
public class TerrainAnalyzer : ScriptableObject
{
    public TerrainAnalysisData AnalyzeTerrain(WorldData data)
    {
        var heightMap = data.TerrainData.TerrainMapResult; 
        var slopeMap = GenerateSlopeMap(heightMap);
        var primaryBiomes = GetPrimaryBiomeMap(data.BiomeMap);
        var biomeWeightsMap = data.BiomeMap;
        var biomeHeightRanges = ComputeBiomeRanges(primaryBiomes, heightMap);

        return new TerrainAnalysisData
            (
                heightMap,
                slopeMap, 
                primaryBiomes, 
                biomeWeightsMap, 
                biomeHeightRanges,
                data.TerrainData
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

    private Dictionary<BiomeConfig, BiomeHeightRange> ComputeBiomeRanges(Map<BiomeConfig> primaryBiomes, Map<float> heightMap)
    {
        var result = new Dictionary<BiomeConfig, BiomeHeightRange>();

        for (int y = 0; y < primaryBiomes.Height; y++)
        {
            for (int x = 0; x < primaryBiomes.Width; x++)
            {
                var biome = primaryBiomes[x, y];
                float height = heightMap[x, y];

                if (biome == null)
                    continue;

                if (!result.TryGetValue(biome, out var range))
                    range = new BiomeHeightRange(height, height);
                else
                    range.Encapsulate(height);

                result[biome] = range;
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
