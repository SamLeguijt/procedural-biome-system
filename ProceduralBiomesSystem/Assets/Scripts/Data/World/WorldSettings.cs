using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldSettings_", menuName = "ScriptableObjects/World/new WorldSettings")]
public class WorldSettings : ScriptableObject
{
    public Vector2 WorldSize;
    public Vector2 ChunkSize;
    public List<BiomeSpawnRule> BiomeSpawnRules; 
}
