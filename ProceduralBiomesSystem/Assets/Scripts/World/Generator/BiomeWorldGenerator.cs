using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeWorldGenerator : IWorldGenerator
{
    public void GenerateWorld(WorldLayout layout, WorldSettings settings)
    {
        // 1) Assign biomes to the chunks 
        // 2) Generate mesh for each chunk (using WorldChunk.ITerrainGenerator
        // 3) Analyze each chunk, store the info in the chunk 
        // 4) Populate each chunk, store the objects in the chunk
        // 5) Blend biomes (optional)

        GameObject parent = new GameObject("WorldMesh");

        foreach (WorldChunk chunk in layout.worldChunks)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            for (int y = 0; y < chunk.Size.y; y++)
            {
                for (int x = 0; x < chunk.Size.x; x++)
                {
                    Vector3 vertice = new Vector3(x * settings.verticeDistance + chunk.WorldPosition.x, 0, y * settings.verticeDistance + chunk.WorldPosition.z);

                    vertices.Add(vertice);
                }
            }

            for (int i = 0; i < vertices.Count - chunk.Size.x; i++)
            {
                // Top triangles
                if ((i + 1) % chunk.Size.x != 0)
                {
                    triangles.Add(i);
                    triangles.Add(i + 1);
                    triangles.Add(i + (int)chunk.Size.x);
                }

                // Bottom triangles
                if (i % chunk.Size.x != 0)
                {
                    triangles.Add(i + (int)chunk.Size.x - 1);
                    triangles.Add(i);
                    triangles.Add(i + (int)chunk.Size.x);
                }
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds(); 


            GameObject chunkObj = new GameObject();
            MeshFilter meshFilter = chunkObj.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = chunkObj.AddComponent<MeshRenderer>();
            meshFilter.mesh = mesh;
            meshRenderer.material = settings.meshMat;
            chunkObj.transform.SetParent(parent.transform);
        }
    }

    private void AssignBiomes(WorldLayout layout)
    {

    }

    private void Analyze(WorldChunk chunk)
    {

    }

    private void Populate(WorldChunk chunk)
    {

    }

    private void BlendBiomeBorders()
    {

    }


}
