using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/new DependencyContainer")]
public class WorldGenerationPreset : ScriptableObject
{
    [SerializeField] public WorldSettings worldSettings;

    [Header("Pipeline Assets")]
    [SerializeField] public ALayoutGenerator layoutGenerator;
    [SerializeField] public ABiomeAssigner biomeAssigner; 
    [SerializeField] public ABiomeTerrainGenerator terrainGenerator;
    [SerializeField] public AWorldGenerator worldGenerator;
    [SerializeField] public WorldAnalyzer worldAnalyzer;
    [SerializeField] public WorldPopulator worldPopulator;
    [SerializeField] public ObjectSpawner spawner = new ObjectSpawner();    

    [SerializeField] public BiomeSet biomeSet;
    [SerializeField] public SeedMode seedMode;
}
