using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTerrainGenerator : ScriptableObject, ITerrainGenerator
{

    public virtual void CreateMesh(WorldChunk chunk)
    {
        BiomeConfig settings = chunk.biomeConfig;

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
                float vertX = -halfSizeX + x * settings.verticeDistance;
                float vertZ = -halfSizeZ + z * settings.verticeDistance;
                Vector2 offset = new Vector2(settings.NoiseSeed, settings.NoiseSeed);
                float vertY = Utils.GetPerlinNoiseValue(new Vector2(vertX, vertZ), settings.NoiseFrequencey, settings.NoiseAmplitude, offset);

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
    public abstract void GenerateTerrain(WorldChunk chunk);
}
