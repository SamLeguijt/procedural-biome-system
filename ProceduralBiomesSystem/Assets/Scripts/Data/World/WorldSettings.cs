using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldSettings_", menuName = "ScriptableObjects/World/new WorldSettings")]
public class WorldSettings : ScriptableObject
{
    [field: SerializeField] public GameObject ChunkPrefab { get; private set; }
    private float recentSeed;

    public float NoiseSeed 
    {
        get
        {
            if (randomSeed)
            {
                recentSeed = Random.Range(0, 1000);
                return recentSeed;
            }
            else
            {
                return ManualSeed;
            }
        }
    }

    public Vector2 WorldSize;
    public Vector2 ChunkSize;


    [Range(0, 1)]
    public float noiseScale = 0.1f;
    public float noiseMultiplier = 10;
    public bool randomSeed = true;
    [SerializeField] private float ManualSeed = 0.01f;

    // TODO: Move mesh settings to another place (per biome?)
    public float verticeDistance = 1;
    public Material meshMat;

    public List<BiomeSpawnRule> BiomeSpawnRules; 
}
