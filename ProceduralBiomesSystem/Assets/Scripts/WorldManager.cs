using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum SeedMode
{
    Random,
    Manual
}

public class WorldManager : MonoBehaviour
{
    [Header("Preset")]
    [SerializeField] private WorldGenerationPreset preset;

    [Header("Visualization")]
    [SerializeField] private MapVisualizer visualizer;

    // Dependencies
    private IWorldGenerator worldGenerator;
    private ALayoutGenerator worldLayoutGenerator;
    private ABiomeAssigner biomeAssigner; 
    private ABiomeTerrainGenerator biomeTerrainGenerator;
    private WorldAnalyzer worldAnalyzer;
    private WorldPopulator worldPopulator;

    private WorldAnalysisData recentAnalysisData = null;
    public static bool AllowRandomSeeds = true;
    List<GameObject> recentWorlds = new List<GameObject>();

    private void GetDependencies()
    {
        worldGenerator = preset.worldGenerator;
        worldLayoutGenerator = preset.layoutGenerator;
        biomeAssigner = preset.biomeAssigner;
        biomeTerrainGenerator = preset.terrainGenerator;
        worldAnalyzer = preset.worldAnalyzer;
        worldPopulator = preset.worldPopulator;

        AllowRandomSeeds = preset.seedMode == SeedMode.Random ? true : false;
        worldLayoutGenerator.OnLayoutChanged += VisualiseMaps;
    }

    private void OnValidate()
    {
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
        if (HasMissingDependencies())
        {
            // Allows outside runtime generation.
            GetDependencies();
        }

        WorldLayout layout = GenerateLayout(preset.worldSettings);
        WorldData world = GenerateWorld(layout);
        
        WorldAnalysisData worldAnalysis = worldAnalyzer.GetAnalysis(world);
        var populations = worldPopulator.PopulateWorld(worldAnalysis);

        GameObject populationParent = new GameObject("PopulationParent");
        CreatePopulation(populations, populationParent);

        GameObject worldObject = CreateWorldObject(world);
        populationParent.transform.SetParent(worldObject.transform, false);

        visualizer.SetRecentLayout(layout);
        visualizer.SetRecentWorld(world);
        recentAnalysisData = worldAnalysis;
    }


    [Button]
    private void ClearWorld()
    {
        foreach (GameObject world in recentWorlds)
            DestroyImmediate(world);
    }

    private WorldLayout GenerateLayout(WorldSettings settings)
    {
        return worldLayoutGenerator.GenerateWorldLayout(settings);
    }

    private WorldData GenerateWorld(WorldLayout layout)
    {
        return worldGenerator.GenerateWorld(layout);
    }


    private GameObject CreateWorldObject(WorldData worldData)
    {
        GameObject world = new GameObject("World");
        MeshFilter meshFilter = world.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = world.AddComponent<MeshRenderer>();

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
            || worldGenerator == null 
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

    // TEMP
    private void OnDrawGizmosSelected()
    {
        if (recentAnalysisData != null)
        {
            var heightMap = recentAnalysisData.TerrainData.HeightMap;
            var slopeMap = recentAnalysisData.TerrainData.SlopeMap;

            for (int y = 0; y < heightMap.Height; y++)
            {
                for (int x = 0; x < heightMap.Width; x++) 
                {
                    float height = heightMap[x, y];
                    float slope = slopeMap[x, y];
                    
                    Vector3 worldPosition = TerrainSpaceUtils.GridToTerrainWorld(x, y, heightMap.Width, heightMap.Height, heightMap[x, y]);

                    if (height > 150)
                    {
                        //Gizmos.color = Color.blue;
                        //Gizmos.DrawSphere(worldPosition, 0.5f);
                    }

                    if (slope >= -0.1 && slope < 0.1f)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireCube(worldPosition, new Vector3(0.5f, 0.1f, 0.5f));
                    }
                }
            }
        }
    }
}
