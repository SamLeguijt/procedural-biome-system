using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABiomeAssigner : ScriptableObject 
{
    public abstract Map<BiomeWeights> GenerateBiomeInfluenceMap(WorldLayout layout, HashSet<BiomeConfig> possibleBiomes);
}
