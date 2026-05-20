using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TEMP DOCUMENTATION
/// This class stores information about the terrain. 
/// Used to describe the generated terrain in its own data format. 
/// </summary>
public class TerrainAnalysisData
{
    public Map<float> HeightMap { get; private set; }
    public Map<float> SlopeMap { get; private set; }
    public Map<BiomeConfig> PrimaryBiomeMap { get; private set; }
    public Map<BiomeWeights> BiomeWeightsMap { get; private set; }

    public int Width => HeightMap.Width;
    public int Height => HeightMap.Height;

    public TerrainAnalysisData(Map<float> heightMap, Map<float> slopeMap, Map<BiomeConfig> primaryBiomeMap, Map<BiomeWeights> biomeWeightsMap)
    {
        HeightMap = heightMap;
        SlopeMap = slopeMap;
        PrimaryBiomeMap = primaryBiomeMap;
        BiomeWeightsMap = biomeWeightsMap;
    }
    public float GetHeight(int x, int y)
    {
        return HeightMap[x, y];
    }

    public float GetSlope(int x, int y)
    {
        return SlopeMap[x, y];
    }

    public BiomeConfig GetPrimaryBiome(int x, int y)
    {
        return PrimaryBiomeMap[x, y];
    }

    public BiomeWeights GetBiomeWeights(int x, int y)
    {
        return BiomeWeightsMap[x, y];
    }
}