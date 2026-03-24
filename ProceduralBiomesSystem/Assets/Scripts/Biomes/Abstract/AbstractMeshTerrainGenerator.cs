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

    [field: SerializeField] public NoiseSettings NoiseSettings { get; private set; }
  
    public Action OnValueChanged;
    private GameObject recentSample = null;

    public float heightMultiplier = 1;
    public abstract void GenerateTerrain(WorldChunk chunk);
        

    private void OnValidate()
    {
        OnValueChanged?.Invoke();

        if (recentSample != null)
        {
            var heightmap = GenerateHeightMap(250, 250);
            Mesh mesh = CreateMesh(heightmap);
            recentSample.GetComponent<MeshFilter>().mesh = mesh;    
        }
    }

    [Button]
    protected void CreateSample()
    {
        GameObject go = new GameObject("TerrainSample");
        MeshRenderer renderer = go.AddComponent<MeshRenderer>();
        MeshFilter filter = go.AddComponent<MeshFilter>();

        var heightmap = GenerateHeightMap(250,250);
        Mesh mesh = CreateMesh(heightmap);

        filter.mesh = mesh;
        renderer.material = MeshMaterial;
        recentSample = go;
    }

    [Button]
    protected void CreateSample2()
    {
        GameObject go = new GameObject("TerrainSample");
        MeshRenderer renderer = go.AddComponent<MeshRenderer>();
        MeshFilter filter = go.AddComponent<MeshFilter>();

        var heightmap = GenerateHeightMapFromNoise(250,250);
        Mesh mesh = CreateMesh(heightmap);

        filter.mesh = mesh;
        renderer.material = MeshMaterial;
    }

    [Button]
    protected void ClearSample()
    {
        if (recentSample != null)
            DestroyImmediate(recentSample);
    }

    public float GetHeightAtWorldPosition(Vector2 worldPos)
    {
        float height = 0;
        float maxHeight = 0;
        float amplitude = 1f;
        float frequency = 1f;

        System.Random random = new System.Random(NoiseSettings.seed);

        Vector2[] octaveOffsets = new Vector2[NoiseSettings.octaves];

        for (int i = 0; i < NoiseSettings.octaves; i++)
        {
            float xOffset = random.Next(-100000, 100000) + NoiseSettings.offset.x;
            float yOffset = random.Next(-100000, 100000) + NoiseSettings.offset.y;
            octaveOffsets[i] = new Vector2(xOffset, yOffset);
        }

        for (int o = 0; o < NoiseSettings.octaves; o++)
        {
            float sampleX = worldPos.x / NoiseSettings.scale * frequency + octaveOffsets[o].x;    
            float sampleY = worldPos.y / NoiseSettings.scale * frequency + octaveOffsets[o].y;

            float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
            height += perlinValue * amplitude;

            maxHeight += amplitude; 
            amplitude *= NoiseSettings.persistance;
            frequency *= NoiseSettings.lacunarity;
        }

        float normalizedHeight = (height + maxHeight) / (2f * maxHeight);

        float finalHeight = NoiseSettings.remapCurve.Evaluate(normalizedHeight);
        return finalHeight ;
    }

    public Map<float> GenerateHeightMapFromNoise(int width, int height)
    {
        Map<float> map = new Map<float>(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 worldPos = new Vector2(x, y); 
                float value = GetHeightAtWorldPosition(worldPos);

                map[x, y] = value;
            }
        }

        return map;
    }

    protected Map<float> GenerateHeightMap(int width, int height)
    {
        int usedSeed = NoiseSettings.seed;
        if (NoiseSettings.useRandomSeed)
            usedSeed = UnityEngine.Random.Range(0, 10000);

        var map = Utils.GenerateNoiseMap(width, height, NoiseSettings);

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
                float height = heightMap[x, z] * heightMultiplier;
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
