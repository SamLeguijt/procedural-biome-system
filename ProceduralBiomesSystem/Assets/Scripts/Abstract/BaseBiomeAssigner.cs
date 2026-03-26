using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBiomeAssigner : ScriptableObject 
{
    [field: SerializeField] protected BiomeSet BiomeSet { get; private set; } 
    public abstract Map<BiomeWeights> GenerateBiomeMap(WorldLayout layout);
}
