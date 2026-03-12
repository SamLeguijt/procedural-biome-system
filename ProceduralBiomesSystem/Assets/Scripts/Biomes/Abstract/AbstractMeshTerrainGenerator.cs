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

    [field: Range(0, 1)]
    [field: SerializeField] public float NoiseFrequencey { get; private set;  } = 0.1f;
    
    [field: SerializeField] public float NoiseAmplitude { get; private set;  } = 10;
    [field: SerializeField] public bool RandomOffset { get; private set; } = true;
    [field: SerializeField] protected Vector2 ManualOffset { get; private set; } = Vector2.zero;

    public int seed;
    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;
    public Vector2 offset; 

    public Vector2 NoiseOffset
    {
        get
        {
            if (RandomOffset)
            {
                int x = Random.Range(0, 1000);
                int y = Random.Range(0, 1000);
                
                return new Vector2(x, y);
            }
            else
            {
                return ManualOffset;
            }
        }
    }
    public abstract void GenerateTerrain(WorldChunk chunk);

    protected float[,] GenerateHeightMap(int width, int height)
    {
        return Utils.GenerateNoiseMap(width, height, seed, scale, octaves, persistance, lacunarity, offset); 
    }

    protected virtual Mesh CreateMesh(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int depth = heightMap.GetLength(1);

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (depth - 1) / 2f;

        Vector3[] vertices = new Vector3[width * depth];
        List<int> triangles = new List<int>();
        int vertexIndex = 0;

        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                vertices[vertexIndex] = new Vector3(topLeftX + x, heightMap[x, z], topLeftZ - z);

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

    protected virtual void CreateMesh(WorldChunk chunk)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        int vertexCountX = ((int)chunk.Size.x / (int)VerticeDistance) + 1;
        int vertexCountZ = ((int)chunk.Size.z / (int)VerticeDistance) + 1;

        float halfSizeX = chunk.Size.x / 2f;
        float halfSizeZ = chunk.Size.z / 2f;

        for (int z = 0; z < vertexCountZ; z++)
        {
            for (int x = 0; x < vertexCountX; x++)
            {
                float vertX = -halfSizeX + x * VerticeDistance;
                float vertZ = -halfSizeZ + z * VerticeDistance;
                float vertY = Utils.GetPerlinNoiseValue(new Vector2(vertX, vertZ), NoiseFrequencey, NoiseAmplitude, NoiseOffset);

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
