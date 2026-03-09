using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldSettings_", menuName = "ScriptableObjects/World/new WorldSettings")]
public class WorldSettings : ScriptableObject
{
    public Vector3 Dimensions;
    public List<BiomeSpawnRule> BiomeSpawnRules; 
}
