using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("Preset")]
    [SerializeField] private WorldGenerationPreset preset;

    [Header("Visualization")]
    [SerializeField] private MapVisualizer visualizer;

    // Dependencies
    private ALayoutGenerator worldLayoutGenerator;
    private ABiomeAssigner biomeAssigner; 
    private ABiomeTerrainGenerator biomeTerrainGenerator;
    private WorldAnalyzer worldAnalyzer;
    private WorldPopulator worldPopulator;

    public static bool AllowRandomSeeds = true;
    private List<GameObject> recentWorlds = new List<GameObject>();

    private void GetDependencies()
    {
        if (preset == null)
        {
            Debug.LogError("[WorldManager] Generation failed --- Missing WorldGenerationPreset reference");
            return;
        }

        worldLayoutGenerator = preset.layoutGenerator;
        biomeAssigner = preset.biomeAssigner;
        biomeTerrainGenerator = preset.terrainGenerator;
        worldAnalyzer = preset.worldAnalyzer;
        worldPopulator = preset.worldPopulator;

        AllowRandomSeeds = preset.seedMode == SeedMode.Random ? true : false;
        worldLayoutGenerator.OnLayoutChanged += VisualiseMaps;
    }

    private void VisualiseMaps(WorldLayout layout)
    {
        if (layout == null)
            return;

        visualizer.SetRecentLayout(layout);
    }

    [Button]
    private void CreateWorld()
    {
        // Allows outside runtime generation.
        if (HasMissingDependencies())
            GetDependencies();

        /// Pipeline steps: 
        
        /// 1) Generate layout of the world
        WorldLayout layout = GenerateLayout(preset.worldSettings);
        
        /// 2) Generate the world based on layout (terrain)
        WorldData world = GenerateWorld(layout);
        
        /// 3) Analyse the generated world
        WorldAnalysisData worldAnalysis = worldAnalyzer.GetAnalysis(world);

        /// 4) Populate the world
        List<PopulationCandidate> populations = worldPopulator.PopulateWorld(worldAnalysis);

        /// 5) Create scene representation
        GameObject worldObject = CreateWorldObject(world, worldAnalysis);
        GameObject populationParent = new GameObject("PopulationParent");
        CreatePopulation(populations, populationParent);
        populationParent.transform.SetParent(worldObject.transform, false);

        visualizer.SetRecentLayout(layout);
        visualizer.SetRecentWorld(world);
    }

    [Button]
    private void ClearWorlds()
    {
        foreach (GameObject world in recentWorlds)
            DestroyImmediate(world);
    }

    private WorldLayout GenerateLayout(WorldSettings settings)
    {
        return worldLayoutGenerator.GenerateWorldLayout(settings);
    }

    public WorldData GenerateWorld(WorldLayout layout)
    {
        Map<float> baseHeightMap = layout.ElevationMap;
        Map<BiomeWeights> rawBiomeMap = biomeAssigner.GenerateBiomeInfluenceMap(layout, preset.biomeSet.Collection);
        Map<float> terrainMap = biomeTerrainGenerator.GenerateTerrainMap(baseHeightMap, rawBiomeMap, preset.biomeSet.Collection);
        Mesh terrainMesh = MeshGenerator.CreateMesh(terrainMap);
        WorldData world = new WorldData(terrainMesh, preset.worldSettings.TerrainMaterial, terrainMap, rawBiomeMap);
        return world;
    }

    private GameObject CreateWorldObject(WorldData worldData, WorldAnalysisData analysis)
    {
        GameObject world = new GameObject("World");
        MeshFilter meshFilter = world.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = world.AddComponent<MeshRenderer>();
        TerrainVisualiser terrainVisualiser = world.AddComponent<TerrainVisualiser>();
        terrainVisualiser.SetWorldData(analysis, worldData.Mesh);

        meshFilter.mesh = worldData.Mesh;
        meshRenderer.material = worldData.Material;

        world.transform.parent = transform;
        recentWorlds.Add(world);
        return world;
    }

    private void CreatePopulation(List<PopulationCandidate> population, GameObject parent)
    {
        foreach (PopulationCandidate candidate in population)
        {
            preset.spawner.SpawnGameObject(candidate.Prefab, candidate.WorldPosition, candidate.Rotation, parent);
        }
    }

    private bool HasMissingDependencies()
    {
        if 
        (      preset == null 
            || worldLayoutGenerator == null
            || biomeAssigner == null
            || biomeTerrainGenerator == null
            || worldAnalyzer == null
            || worldPopulator == null
        )
        {
            return true;
        }

        return false;
    }
}
