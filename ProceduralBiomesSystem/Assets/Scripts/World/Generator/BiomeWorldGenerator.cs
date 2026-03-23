using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float verticeDistance = 1;

    private Dictionary<EBiome, BiomeConfig> biomeConfigMappings = new Dictionary<EBiome, BiomeConfig>();

    public int meshResolution = 1;
    public float noiseScale = 0.05f;

    public override World GenerateWorld(WorldLayout layout)
    {
        Map<float> biomeInfluenceHeightMap = BiomeToHeightMap(layout.BiomeMap);

        // Combine maps
        Map<float> finalHeightMap = AddMaps(layout.ElevationMap, biomeInfluenceHeightMap) ;

        Mesh terrainMesh = CreateMesh(finalHeightMap.Values);
        Color[] colorMap = BiomeToColorMap(layout.BiomeMap, terrainMesh.vertices.Length);

        terrainMesh.colors = colorMap;

        // Analyze... (in generator?)
        // Populate... (in generator?)

        return new World(terrainMesh, terrainMaterial);
    }

    public Map<float> AddMaps(Map<float> mapA, Map<float> mapB)
    {
        int mapWidth = mapA.Width;
        int mapHeight = mapB.Height;

        Map<float> heightMapResult = new Map<float>(mapWidth, mapHeight);

        float biomeInfluence = 0.05f; /// MAKE MEMBER

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float height = mapA[x,y] + (mapB[x,y] * biomeInfluence);

                heightMapResult[x,y] = height;
            }
        }

        return heightMapResult;
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
                Vector2 worldPos = new Vector2(x, y); 

                float mountainValue = GetBiomeNoiseValue(worldPos, EBiome.Mountains);
                float volcanicValue = GetBiomeNoiseValue(worldPos, EBiome.Volcanic);
                float desertValue = GetBiomeNoiseValue(worldPos, EBiome.Desert);
                float plainsValue = GetBiomeNoiseValue(worldPos, EBiome.Plains);
                
                float finalHeight = (mountainValue * biome.MountainsWeight)
                                    + (volcanicValue * biome.VolcanicWeight)
                                    + (desertValue * biome.DesertWeight)
                                    + (plainsValue * biome.PlainsWeight);

  
                heightMap[x, y] = finalHeight;
            }
        }

        return heightMap;
    }

    private float GetBiomeNoiseValue(Vector2 worldPos, EBiome biomeType)
    {
        BiomeConfig config = GetBiomeData(biomeType);
        float noiseValue = config.Generator.GetHeightAtWorldPosition(worldPos);

        return noiseValue;
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
                    color = biomeMap[x,y].ToColor();
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
