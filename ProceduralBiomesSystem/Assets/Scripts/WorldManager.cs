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
    [Header("Dependencies")]
    [SerializeField] private WorldSettings worldSettings;
    [SerializeField] private DependencyContainer demoContainer;
    [SerializeField] private WorldAnalyzer worldAnalyzer;
    [SerializeField] private WorldPopulator worldPopulator;

    [SerializeField] private SeedMode seedMode;
    public static bool AllowRandomSeeds = true;

    [Header("Visualization")]
    [SerializeField] private MapVisualizer visualizer;

    private IWorldGenerator worldGenerator;
    private AbstractLayoutGenerator worldLayoutGenerator;

    List<GameObject> recentWorlds = new List<GameObject>();

    private void GetDependencies()
    {
        worldGenerator = worldSettings.WorldGenerator;
        worldLayoutGenerator = worldSettings.LayoutGenerator;
        worldLayoutGenerator.OnLayoutChanged += VisualiseMaps; 
    }

    private void OnValidate()
    {
        AllowRandomSeeds = seedMode == SeedMode.Random ? true : false;
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
        if (CheckForNull())
        {
            // Allows outside runtime generation.
            GetDependencies();
        }

        WorldLayout layout = GenerateLayout(worldSettings);
        WorldData world = GenerateWorld(layout);
        
        // TODO: 
        // Analyse and Populate steps
        WorldAnalysisData worldAnalysis = worldAnalyzer.GetAnalysis(world);
        var populations = worldPopulator.PopulateWorld(worldAnalysis);

        CreateWorldObject(world);

        visualizer.SetRecentLayout(layout);
        visualizer.SetRecentWorld(world);
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


    private void CreateWorldObject(WorldData worldData)
    {
        GameObject world = new GameObject("World");
        MeshFilter meshFilter = world.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = world.AddComponent<MeshRenderer>();

        meshFilter.mesh = worldData.Mesh;
        meshRenderer.material = worldData.Material;

        world.transform.parent = transform;
        recentWorlds.Add(world);
    }

    private bool CheckForNull()
    {
        if (worldSettings == null || worldLayoutGenerator == null || worldGenerator == null)
        {
            return true;
        }

        return false;
    }
}
