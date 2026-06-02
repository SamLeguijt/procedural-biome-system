using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Refactor construction and what this class should/shouldnt store
public class TerrainData
{
    public Dictionary<BiomeConfig, Map<float>> BiomeTerrainMaps { get; private set; }
    public Map<float> BiomeBlendMap { get; private set; }
    public Map<float> BaseHeightMap { get; private set; }
    public Map<float> TerrainMapResult { get; private set; }

    public TerrainData(Dictionary<BiomeConfig, Map<float>> biomeTerrainMaps, Map<float> biomeBlendMap, Map<float> baseHeightMap, Map<float> result)
    {
        BiomeTerrainMaps = biomeTerrainMaps;
        BiomeBlendMap = biomeBlendMap;
        BaseHeightMap = baseHeightMap;
        TerrainMapResult = result;
    }
}
