using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldSettings_", menuName = "ScriptableObjects/World/new WorldSettings")]
public class WorldSettings : ScriptableObject
{
    [field: SerializeField] public GameObject ChunkPrefab { get; private set; }

    public Vector2 WorldSize;
    public Vector2 ChunkSize;

    // TODO: Move mesh settings to another place (per biome?)
    public float verticeDistance = 1;
    public Material meshMat;

    public List<BiomeSpawnRule> BiomeSpawnRules; 
}
