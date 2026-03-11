using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private WorldSettings worldSettings;

    [Header("Debug settings")]
    [SerializeField] private bool drawChunkGizmos = true;
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

        if (layout.worldChunks.Count == 0)
            return;

        GameObject recentWorld = new GameObject("World");
        for (int i = 0; i < layout.worldChunks.Count; i++)
        {
            WorldChunk chunk = layout.worldChunks[i];
            GameObject chunkObject = Instantiate(worldSettings.ChunkPrefab, chunk.WorldPosition, Quaternion.identity, recentWorld.transform);
            MeshFilter meshFilter = chunkObject.GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = chunkObject.GetComponent<MeshRenderer>();
            meshFilter.mesh = chunk.mesh;
            meshRenderer.material = chunk.biomeConfig.meshMaterial;
        }

        recentWorlds.Add(recentWorld);
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
                foreach (WorldChunk chunk in recentLayoutDebug.worldChunks)
                {   
                    if (drawChunkGizmos)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(chunk.WorldPosition, .5f);
                    }

                    if (chunk.mesh == null)
                        continue;

                    if (drawVerticeGizmos)
                    {
                        Gizmos.color = Color.green;

                        Vector3[] verts = chunk.mesh.vertices;
                        int[] tris = chunk.mesh.triangles;

                        for (int i = 0; i < tris.Length; i += 3)
                        {
                            Vector3 a = verts[tris[i]] + chunk.WorldPosition;
                            Vector3 b = verts[tris[i + 1]] + chunk.WorldPosition;
                            Vector3 c = verts[tris[i + 2]] + chunk.WorldPosition;

                            Gizmos.DrawLine(a, b);
                            Gizmos.DrawLine(b, c);
                            Gizmos.DrawLine(c, a);
                        }
                    }
                }
            }
        }
    }
}
