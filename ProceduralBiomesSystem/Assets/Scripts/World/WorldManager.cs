using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private WorldSettings worldSettings;

    private IWorldGenerator worldGenerator;
    private IWorldLayoutGenerator worldLayoutGenerator;

    // TODO: Make seperate visualisation script(s).
    WorldLayout recentLayoutDebug;

    // TODO: Factory / DI.
    private void GetDependencies()
    {
        worldGenerator = new BiomeWorldGenerator();
        worldLayoutGenerator = new GridLayoutGenerator();
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
    }


    private WorldLayout GenerateLayout(WorldSettings settings)
    {
        return worldLayoutGenerator.GenerateWorldLayout(settings);
    }

    private void GenerateWorld(WorldLayout layout)
    {
        worldGenerator.GenerateWorld(layout, worldSettings);
    }

    private bool CheckForNull()
    {
        if (worldSettings == null)
        {
            Debug.LogError("[WorldGenerator] WorldSettings is null!");
            return true;
        }

        if (worldGenerator == null)
        {
            Debug.LogError("[WorldGenerator] WorldGenerator is null!");
            return true;
        }

        if (worldLayoutGenerator == null)
        {
            Debug.LogError("[WorldGenerator] WorldLayoutGenerator is null!");
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (recentLayoutDebug != null)
        {
            if (recentLayoutDebug.worldChunks != null)
            {
                foreach (var item in recentLayoutDebug.worldChunks)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireCube(item.WorldPosition, new Vector3(item.Size.x, item.Size.y, item.Size.x));
                }
            }
        }
    }
}
