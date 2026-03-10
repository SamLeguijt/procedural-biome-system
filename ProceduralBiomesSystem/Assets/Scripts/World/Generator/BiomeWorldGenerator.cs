using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        foreach (WorldChunk chunk in layout.worldChunks)
        {
            CreateMesh(chunk, settings); 
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

    private void CreateMesh(WorldChunk chunk, WorldSettings settings)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        int vertexCountX = ((int)chunk.Size.x / (int)settings.verticeDistance) + 1;
        int vertexCountZ = ((int)chunk.Size.z / (int)settings.verticeDistance) + 1;

        float halfSizeX = chunk.Size.x / 2f;
        float halfSizeZ = chunk.Size.z / 2f;

        for (int z = 0; z < vertexCountZ; z++)
        {
            for (int x = 0; x < vertexCountX; x++)
            {
                float vertX = - halfSizeX + x * settings.verticeDistance;
                float vertY = 0;
                float vertZ = - halfSizeZ + z * settings.verticeDistance;

                Vector3 vertice = new Vector3(vertX, vertY, vertZ);
                vertices.Add(vertice);
            }
        }

        // 2) Generate triangles
        for (int y = 0; y < vertexCountZ - 1; y++)
        {
            for (int x = 0; x < vertexCountX - 1; x++)
            {
                int topLeft = y * vertexCountX + x;
                int topRight = topLeft + 1;
                int bottomLeft = topLeft + vertexCountX;
                int bottomRight = bottomLeft + 1;

                triangles.Add(topLeft);
                triangles.Add(bottomLeft);
                triangles.Add(topRight);

                triangles.Add(topRight);
                triangles.Add(bottomLeft);
                triangles.Add(bottomRight);
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        chunk.mesh = mesh;
    }
}
