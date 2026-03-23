using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


/// <summary>
/// Generates a world of biomes, where each biome is responsible for generating it's own terrain. 
/// </summary>
[CreateAssetMenu(fileName = "WorldGenerator_", menuName = "ScriptableObjects/World/new WorldGenerator")]
public class BiomeWorldGenerator : AbstractWorldGenerator
{
    [field: SerializeField] public List<BiomeConfig> BiomeConfigs { get; private set; }

    [field: SerializeField] private Material terrainMaterial = null;
    [SerializeField] int meshWidth = 200;  
    [SerializeField] int meshHeight = 200;

    public int heightMultiplier = 10;

    private Dictionary<EBiome, BiomeConfig> biomeConfigMappings = new Dictionary<EBiome, BiomeConfig>();

    public int meshResolution = 1;
    public float noiseScale = 0.05f;

    public override World GenerateWorld(WorldLayout layout)
    {
        Map<float> heightMap = BiomeToHeightMap(layout.BiomeMap);

        Mesh terrainMesh = CreateMesh(layout.ElevationMap.Values);
        Color[] colorMap = BiomeToColorMap(layout.BiomeMap, terrainMesh.vertices.Length);

        terrainMesh.colors = colorMap;

        // Analyze... (in generator?)
        // Populate... (in generator?)

        return new World(terrainMesh, terrainMaterial);
    }

    private Map<float> BiomeToHeightMap(Map<BiomeWeights> biomeMap)
    {
        int width = biomeMap.Width;
        int height = biomeMap.Height;

        Map<float> heightMap = new Map<float>(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                BiomeWeights biome = biomeMap[x, y];

                /// For each biome, get a noise value using the configs generators.
                /// Use the weights to add all the noise values together
                /// Normalise that all to 0-1
                /// Return it all as a float map.
                /// 

                BiomeConfig mountainConfig = GetBiomeData(EBiome.Mountains);
                BiomeConfig volcanicConfig = GetBiomeData(EBiome.Volcanic);
                BiomeConfig desertConfig  = GetBiomeData(EBiome.Desert);
                BiomeConfig plainsConfig  = GetBiomeData(EBiome.Plains);


                
                    //heightMap[x, y] = config.Generator.GetHeightAtWorldPosition(worldPos);
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
        int mapWidth = heightMap.GetLength(0);
        int mapHeight = heightMap.GetLength(1);

        Vector3[] vertices = new Vector3[mapWidth * mapHeight];

        List<int> triangles = new List<int>();

        float topLeftX = (mapWidth - 1) / -2f;
        float topLeftZ = (mapHeight - 1) / 2f;

        int vertexIndex = 0;

        for (int z = 0; z < mapHeight ; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float vertexHeight = heightMap[x, z] * heightMultiplier;
                vertices[vertexIndex] = new Vector3(topLeftX + x, vertexHeight, topLeftZ - z);

                if (x < mapWidth- 1 && z < mapHeight - 1)
                {
                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + mapWidth + 1);
                    triangles.Add(vertexIndex + mapWidth);

                    triangles.Add(vertexIndex + mapWidth + 1);
                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; 
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    Color[] BiomeToColorMap(Map<BiomeWeights> biomeMap, int verticesCount)
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
                    //foreach (var dict in biomeMap[x, y])
                    //{
                    //    switch (dict.Key)
                    //    {
                    //        case EBiome.Desert:
                    //            color = Color.yellow;
                    //            break;
                    //        case EBiome.Mountains:
                    //            color = Color.gray;
                    //            break;
                    //        case EBiome.Volcanic:
                    //            color = Color.red;
                    //            break;
                    //        case EBiome.Plains:
                    //            color = Color.green;
                    //            break;
                    //    }
                    //}
                }

                colorMap[currentIndex] = new Color(color.r, color.g, color.b, color.a);
                currentIndex++; 
            }
        }

        return colorMap;
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
