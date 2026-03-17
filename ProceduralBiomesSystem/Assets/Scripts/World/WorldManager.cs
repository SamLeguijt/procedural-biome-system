using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private WorldSettings worldSettings;

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

    [Button]
    private void CreateWorld()
    {
        if (CheckForNull())
        {
            // Allows outside runtime generation.
            GetDependencies();
        }

        WorldLayout layout = GenerateLayout(worldSettings);
        GenerateWorld(layout);
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

    private void GenerateWorld(WorldLayout layout)
    {
        worldGenerator.GenerateWorld(layout);
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
