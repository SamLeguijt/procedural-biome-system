using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private WorldSettings worldSettings;

    [Header("Visualization")]
    [SerializeField] private MapVisualizer visualizer;
    [SerializeField] private MapDrawMode debugMap;


    [Header("Debug settings")]
    [SerializeField] private bool drawLayoutGizmos = true;
    [SerializeField] private bool drawVerticeGizmos = true;

    private IWorldGenerator worldGenerator;
    private IWorldLayoutGenerator worldLayoutGenerator;

    // TODO: Make seperate visualisation script(s).
    WorldLayout recentLayoutDebug;
    List<GameObject> recentWorlds = new List<GameObject>();

    // TODO: Factory / DI.
    private void GetDependencies()
    {
        worldGenerator = worldSettings.WorldGenerator;
        worldLayoutGenerator = worldSettings.LayoutGenerator;
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
            return;

        //CreateWorld();

        if (recentLayoutDebug == null)
            return;

        switch (debugMap)
        {
            case MapDrawMode.Elevation:
                visualizer.DrawFloatMap(recentLayoutDebug.ElevationMap);
                break;

            case MapDrawMode.Erosion:
                visualizer.DrawFloatMap(recentLayoutDebug.ErosionMap);
                break;

            case MapDrawMode.Humidity:
                visualizer.DrawFloatMap(recentLayoutDebug.HumidityMap);
                break;

            case MapDrawMode.Biomes:
                visualizer.DrawBiomeMap(recentLayoutDebug.BiomeMap);
                break;
            case MapDrawMode.All:
                visualizer.DrawFloatMap(recentLayoutDebug.ElevationMap);
                visualizer.DrawFloatMap(recentLayoutDebug.ErosionMap);
                visualizer.DrawFloatMap(recentLayoutDebug.HumidityMap);
                break;
        }

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
        World world = GenerateWorld(layout);
        
        CreateWorldObject(world);

        recentLayoutDebug = layout;
    }


    [Button]
    private void ClearWorld()
    {
        recentLayoutDebug = null;
        foreach (GameObject world in recentWorlds)
            DestroyImmediate(world);
    }

    private WorldLayout GenerateLayout(WorldSettings settings)
    {
        return worldLayoutGenerator.GenerateWorldLayout(settings);
    }

    private World GenerateWorld(WorldLayout layout)
    {
        return worldGenerator.GenerateWorld(layout);
    }


    private void CreateWorldObject(World worldData)
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

    private void OnDrawGizmos()
    {
        if (!drawLayoutGizmos)
            return;

        if (recentLayoutDebug != null && recentLayoutDebug.BiomeMap != null)
        {
            int width = recentLayoutDebug.BiomeMap.Width;
            int depth = recentLayoutDebug.BiomeMap.Height;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < depth; y++)
                {
                    Color color;

                    switch (recentLayoutDebug.BiomeMap[x, y])
                    {
                        case EBiome.Desert:
                            color = Color.yellow;
                            break;
                        case EBiome.Mountains:
                            color = Color.green;
                            break;
                        case EBiome.Volcanic:
                            color = Color.red;
                            break;
                        default:
                            color = Color.black;
                            break;
                    }

                    Gizmos.color = color;

                    float topLeftX = (width - 1) / -2f;
                    float topLeftZ = (depth - 1) / 2f;

                    Vector3 pos = new Vector3(x + topLeftX, 1 * 10, topLeftZ - y);
                    Gizmos.DrawCube(pos, Vector3.one * 0.9f);
                }
            }

            return;
        }
    }
}
