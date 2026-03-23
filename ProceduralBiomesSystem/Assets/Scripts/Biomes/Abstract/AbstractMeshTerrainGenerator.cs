using System;
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

    public Action OnValueChanged;

    public abstract void GenerateTerrain(WorldChunk chunk);


    private void OnValidate()
    {
        OnValueChanged?.Invoke();
    }

    public float GetHeightAtWorldPosition(Vector2 worldPos)
    {
        float height = 0;
        float maxHeight = 0;
        float amplitude = 1f;
        float frequency = 1f;

        System.Random random = new System.Random(seed);

        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            float xOffset = random.Next(-100000, 100000) + offset.x;
            float yOffset = random.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(xOffset, yOffset);
        }

        for (int o = 0; o < octaves; o++)
        {
            float sampleX = worldPos.x / scale * frequency + octaveOffsets[o].x;    
            float sampleY = worldPos.y / scale * frequency + octaveOffsets[o].y;

            float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
            height += perlinValue * amplitude;

            maxHeight += amplitude; 
            amplitude *= persistance;
            frequency *= lacunarity;
        }

        float normalizedHeight = (height + maxHeight) / (2f * maxHeight);

        float finalHeight = heightCurve.Evaluate(normalizedHeight);
        return finalHeight ;
    }

    public Map<float> GenerateHeightMapFromNoise(int width, int height)
    {
        Map<float> map = new Map<float>(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 worldPos = new Vector2(x, y); // grid space
                float value = GetHeightAtWorldPosition(worldPos);

                map[x, y] = value;
            }
        }

        return map;
    }

    protected Map<float> GenerateHeightMap(int width, int height)
    {
        int usedSeed = seed;
        if (randomSeed)
            usedSeed = UnityEngine.Random.Range(0, 10000);

        //var map = Utils.GenerateNoiseMap(width, height, usedSeed, scale, octaves, persistance, lacunarity, offset);

        return new Map<float>(0,0);
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
