using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBiomeAssigner : ScriptableObject 
{
    public abstract Map<BiomeWeights> GenerateBiomeMap(WorldLayout layout, List<BiomeConfig> possibleBiomes);
}
