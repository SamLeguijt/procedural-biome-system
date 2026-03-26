using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseBiomeTerrainGenerator : ScriptableObject
{
    public abstract Map<float> GenerateTerrainMap(Map<float> baseHeightMap, Map<BiomeWeights> biomeWeightsMap);
}
