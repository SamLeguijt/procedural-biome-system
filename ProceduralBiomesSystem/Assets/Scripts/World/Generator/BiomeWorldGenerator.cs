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
    [SerializeField] int meshWidth = 200;  
    [SerializeField] int meshHeight = 200;

    private Dictionary<EBiome, BiomeConfig> biomeConfigMappings = new Dictionary<EBiome, BiomeConfig>();

    public int meshResolution = 1;
    public float noiseScale = 0.05f;

    public override World GenerateWorld(WorldLayout layout)
    {
        Map<float> heightMap = BiomeToHeightMap(layout.BiomeMap);

        Mesh terrainMesh = CreateMesh(heightMap.Values);
        //Color[] colorMap = BiomeToColorMap(layout.BiomeMap, terrainMesh.vertices.Length);

        //terrainMesh.colors = colorMap;

        // Analyze... (in generator?)
        // Populate... (in generator?)

        return new World(terrainMesh, terrainMaterial);
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

                Vector2 worldPos = new Vector2(
                    x * noiseScale,
                    y * noiseScale
                ); 
                
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
        int mapWidth = heightMap.GetLength(0);
        int mapHeight = heightMap.GetLength(1);

        //int verticesX = (width / meshResolution) +1;
        //int verticesY = (height / meshResolution) +1;

        Vector3[] vertices = new Vector3[meshWidth * meshHeight];
        //Color[] vertexColors = new Color[verticesX * verticesY];

        List<int> triangles = new List<int>();

        float topLeftX = (meshWidth - 1) / -2f;
        float topLeftZ = (meshHeight - 1) / 2f;

        int vertexIndex = 0;

        for (int z = 0; z < meshHeight ; z++)
        {
            for (int x = 0; x < meshWidth; x++)
            {
                float percentX = x / (float)(meshWidth - 1);
                float percentZ = z / (float)(meshHeight - 1);

                // Map to heightMap
                int mapX = Mathf.RoundToInt(percentX * (mapWidth - 1));
                int mapZ = Mathf.RoundToInt(percentZ * (mapHeight - 1));
                //int mapX = Mathf.Min(x * meshResolution, width -1);
                //int mapZ = Mathf.Min(z * meshResolution, height -1);
                
                float vertexHeight = heightMap[mapX, mapZ];
                vertices[vertexIndex] = new Vector3(topLeftX + x, vertexHeight, topLeftZ - z);

                if (x < meshWidth - 1 && z < meshHeight - 1)
                {
                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + meshWidth + 1);
                    triangles.Add(vertexIndex + meshWidth);

                    triangles.Add(vertexIndex + meshWidth + 1);
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
