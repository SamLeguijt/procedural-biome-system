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
    [field: SerializeField] public float HeightBaseline { get; private set; } = 0.0f;
    [field: SerializeField] public float HeightMultiplier { get; private set; } = 1;

    public Action OnValueChanged;
    private GameObject recentSample = null;

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
    protected void ClearSample()
    {
        if (recentSample != null)
            DestroyImmediate(recentSample);
    }

    public Map<float> GenerateHeightMap(int width, int height)
    {
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
                float height = heightMap[x, z] * HeightMultiplier;
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
