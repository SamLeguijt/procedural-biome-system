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

    public AnimationCurve heightCurve; 
    public bool randomSeed = false; 
    public int seed;
    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;
    public float heightMultiplier = 1; 
    public Vector2 offset;

    Map<float> noiseMap = null;

    public abstract void GenerateTerrain(WorldChunk chunk);

    public float GetHeightAtWorldPosition(Vector2 worldPos)
    {
        float height = 0;
        float amplitude = 1f;
        float frequency = 1f;
        for (int o = 0; o < octaves; o++)
        {
            float sampleX = worldPos.x / scale * frequency + offset.x;
            float sampleY = worldPos.y / scale * frequency + offset.y;

            float perlin = Mathf.PerlinNoise(sampleX + seed, sampleY + seed) * 2 - 1;
            height += perlin * amplitude;

            amplitude *= persistance;
            frequency *= lacunarity;
        }

        return heightCurve.Evaluate(height) * heightMultiplier;
    }

    public float GetHeightAtPosition(int x, int y, int mapWidth, int mapHeight)
    {
        if (noiseMap == null)
        {
            noiseMap = GenerateHeightMap(mapWidth, mapHeight);
        }

        float result = heightCurve.Evaluate(noiseMap[x, y]) * heightMultiplier;

        return result;
    }

    protected Map<float> GenerateHeightMap(int width, int height)
    {
        int usedSeed = seed;
        if (randomSeed)
            usedSeed = Random.Range(0, 10000);

        var map = Utils.GenerateNoiseMap(width, height, usedSeed, scale, octaves, persistance, lacunarity, offset);

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
                float height = heightCurve.Evaluate(heightMap[x, z]) * heightMultiplier;
                vertices[vertexIndex] = new Vector3(topLeftX + x, height, topLeftZ - z);
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
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;       
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }
}
