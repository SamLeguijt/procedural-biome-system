using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;

public abstract class AbstractMeshTerrainGenerator : ScriptableObject, ITerrainGenerator
{
    [field: Header("Mesh generation:")]
    [field: SerializeField] public Material MeshMaterial { get; private set; }
    [field: SerializeField] public float VerticeDistance { get; private set; } = 1;

    public bool randomSeed = false; 
    public int seed;
    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;
    public float heightMultiplier = 1; 
    public Vector2 offset; 

    public abstract void GenerateTerrain(WorldChunk chunk);

    protected Map<float> GenerateHeightMap(WorldChunk chunk)
    {
        int usedSeed = seed;
        if (randomSeed)
            usedSeed = Random.Range(0, 10000);
        var map = Utils.GenerateNoiseMap(chunk.Quads.x +1 , chunk.Quads.y +1, usedSeed, scale, octaves, persistance, lacunarity, offset + new Vector2(chunk.WorldPosition.x, chunk.WorldPosition.z)); 
        
        return new Map<float>(map);
    }

    protected virtual Mesh CreateMesh(Map<float> heightMap)
    {
        int width = heightMap.Width;
        int depth = heightMap.Height;

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (depth - 1) / 2f;

        Vector3[] vertices = new Vector3[width * depth];
        List<int> triangles = new List<int>();
        int vertexIndex = 0;

        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                vertices[vertexIndex] = new Vector3(topLeftX + x, heightMap[x, z] * -heightMultiplier, topLeftZ - z);
                if (x < width - 1 && z < depth - 1)
                {
                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + width + 1);
                    triangles.Add(vertexIndex + width);

                    triangles.Add(vertexIndex + width + 1);
                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + 1);
                }
                vertexIndex++;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }
}
