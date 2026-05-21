using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ABiomeTerrainGenerator : ScriptableObject
{
    public abstract Map<float> GenerateTerrainMap(Map<float> baseHeightMap, Map<BiomeWeights> biomeWeightsMap);
}
