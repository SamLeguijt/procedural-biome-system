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
            Mesh mesh = MeshGenerator.CreateMesh(heightmap);
            recentSample.GetComponent<MeshFilter>().mesh = mesh;    
        }
    }

    [Button]
    protected void CreateSample()
    {
        var heightmap = GenerateHeightMap(250,250);
        Mesh mesh = MeshGenerator.CreateMesh(heightmap);

        GameObject go = Utils.CreateGameObjectFromMesh(mesh, MeshMaterial, "TerrainSample");
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
}
