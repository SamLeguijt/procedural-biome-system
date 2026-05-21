using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/new DependencyContainer")]
public class WorldGenerationPreset : ScriptableObject
{
    [Header("Global Settings")]
    [SerializeField] public WorldSettings worldSettings;
    [SerializeField] public BiomeSet biomeSet;
    [SerializeField] public SeedMode seedMode;

    [Header("Pipeline steps")]
    [SerializeField] public ALayoutGenerator layoutGenerator;
    [SerializeField] public ABiomeAssigner biomeAssigner; 
    [SerializeField] public ABiomeTerrainGenerator terrainGenerator;
    [SerializeField] public WorldAnalyzer worldAnalyzer;
    [SerializeField] public WorldPopulator worldPopulator;
    [SerializeField] public ObjectSpawner spawner = new ObjectSpawner();    
}
