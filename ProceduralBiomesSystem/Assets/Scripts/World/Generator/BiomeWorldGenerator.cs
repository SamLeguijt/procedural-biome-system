using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Random = UnityEngine.Random;


/// <summary>
/// Generates a world of biomes, where each biome is responsible for generating it's own terrain. 
/// </summary>
[CreateAssetMenu(fileName = "WorldGenerator_", menuName = "ScriptableObjects/World/new WorldGenerator")]
public class BiomeWorldGenerator : AbstractWorldGenerator
{
    [field: SerializeField] public List<BiomeConfig> BiomeConfigs { get; private set; }

    [field: SerializeField] private Material terrainMaterial = null;

    private Dictionary<EBiome, BiomeConfig> biomeConfigMappings = new Dictionary<EBiome, BiomeConfig>();

    public int meshResolution = 1;

    public override void GenerateWorld(WorldLayout layout)
    {
        if (BiomeConfigs.Count == 0)
            return;

        Map<float> heightMap = BiomeToHeightMap(layout.BiomeMap);

        Mesh terrainMesh = CreateMesh(heightMap.Values);
        //Color[] colorMap = BiomeToColorMap(layout.BiomeMap, terrainMesh.vertices.Length);

        //terrainMesh.colors = colorMap;
        CreateMeshObject(terrainMesh);

        // Analyze... (in generator?)
        // Populate... (in generator?)
    }

    private Map<float> BiomeToHeightMap(Map<EBiome> biomeMap)
    {
        int width = biomeMap.Width;
        int height = biomeMap.Height;

        Map<float> heightMap = new Map<float>(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                EBiome biome = biomeMap[x, y];
                BiomeConfig config = GetBiomeData(biome);

                Vector2 worldPos = new Vector2(x, y);
                if (config != null)
                    heightMap[x, y] = config.Generator.GetHeightAtWorldPosition(worldPos);
            }
        }

        return heightMap;
    }

    private BiomeConfig GetBiomeData(EBiome biomeType)
    {
        if (biomeConfigMappings.ContainsKey(biomeType))
        {
            BiomeConfig biomeData = biomeConfigMappings[biomeType];

            if (biomeData != null)
                return biomeData;
        }

        foreach (BiomeConfig config in BiomeConfigs)
        {
            if (config.BiomeType == biomeType)
            {
                biomeConfigMappings.Add(biomeType, config);
                return config;
            }
        }

        /// Todo: Throw error?
        return null;
    }

    private Mesh CreateMesh(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        int verticesX = (width / meshResolution) +1;
        int verticesY = (height / meshResolution) +1;

        Vector3[] vertices = new Vector3[verticesX * verticesY];
        Color[] vertexColors = new Color[verticesX * verticesY];

        List<int> triangles = new List<int>();

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int vertexIndex = 0;

        for (int z = 0; z < verticesY ; z++)
        {
            for (int x = 0; x < verticesX; x++)
            {
                int mapX = Mathf.Min(x * meshResolution, width -1);
                int mapZ = Mathf.Min(z * meshResolution, height -1);
                
                float vertexHeight = heightMap[mapX, mapZ];
                vertices[vertexIndex] = new Vector3(topLeftX + x, vertexHeight, topLeftZ - z);

                if (x < verticesX - 1 && z < verticesY - 1)
                {
                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + verticesX + 1);
                    triangles.Add(vertexIndex + verticesX);

                    triangles.Add(vertexIndex + verticesX + 1);
                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    Color[] BiomeToColorMap(Map<EBiome> biomeMap, int verticesCount)
    {
        Color[] colorMap = new Color[verticesCount];

        int currentIndex = 0;

        int width = biomeMap.Width;
        int height = biomeMap.Height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color color = Color.white;

                if (biomeMap.Contains(x, y))
                {
                    switch (biomeMap[x, y])
                    {
                        case EBiome.Desert:
                            color = Color.yellow;
                            break;
                        case EBiome.Mountains:
                            color = Color.green;
                            break;
                        case EBiome.Volcanic:
                            color = Color.red;
                            break;
                    }
                }

                colorMap[currentIndex] = color;
                currentIndex++; 
            }
        }

        return colorMap;
    }

    private void CreateMeshObject(Mesh mesh)
    {
        GameObject world = new GameObject("WORLD");
        MeshFilter meshFilter = world.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = world.AddComponent<MeshRenderer>();

        meshFilter.mesh = mesh;
        meshRenderer.material = terrainMaterial;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
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
