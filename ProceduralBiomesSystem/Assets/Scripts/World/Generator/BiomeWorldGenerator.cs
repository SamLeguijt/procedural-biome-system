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
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


[CreateAssetMenu(fileName = "WorldGenerator_", menuName = "ScriptableObjects/World/new WorldGenerator")]
public class BiomeWorldGenerator : AbstractWorldGenerator
{
    [field: SerializeField] public List<BiomeConfig> BiomeConfigs { get; private set; }

    [field: SerializeField] private Material terrainMaterial = null;
    [SerializeField] int meshWidth = 200;
    [SerializeField] int meshHeight = 200;

    public int heightMultiplier = 10;
    [Range(0, 1)]
    public float biomeInfluence = 1f;

    private Dictionary<EBiome, BiomeConfig> biomeConfigMappings = new Dictionary<EBiome, BiomeConfig>();

    public override World GenerateWorld(WorldLayout layout)
    {
        Map<float> baseHeightMap = layout.ElevationMap;

        var biomeHeightMaps = GenerateBiomeTerrainMaps(layout.BiomeMap.Width, layout.BiomeMap.Height);
        var blendedMap = BlendBiomeMaps(layout.BiomeMap, biomeHeightMaps);

        //CreateBiomeTerrainMapsDebug(biomeHeightMaps);

        Map<float> finalHeightmap = blendedMap; //CombineMaps(baseHeightMap, blendedMap);
        finalHeightmap = Normalize(finalHeightmap);

        Mesh terrainMesh = CreateMesh(finalHeightmap.Values);
        Color[] colorMap = BiomeToColorMap(layout.BiomeMap, terrainMesh.vertices.Length);

        terrainMesh.colors = colorMap;

        // Analyze... (in generator?)
        // Populate... (in generator?)

        return new World(terrainMesh, terrainMaterial);
    }

    private void CreateBiomeTerrainMapsDebug(Dictionary<EBiome, Map<float>> maps)
    {
        foreach (var kvp in maps)
        {
            var map = kvp.Value;

            Mesh mesh = CreateMesh(map.Values);

            GameObject go = new GameObject("Biome terrain debug");
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            MeshFilter filter = go.AddComponent<MeshFilter>();

            filter.sharedMesh = mesh;
            renderer.material = terrainMaterial;
        }
    }


    private Dictionary<EBiome, Map<float>> GenerateBiomeTerrainMaps(int width, int height)
    {
        var maps = new Dictionary<EBiome, Map<float>>();

        foreach (var config in BiomeConfigs)
        {
            var generator = config.Generator;
            var map = generator.GenerateHeightMap(width, height);
            maps.Add(config.BiomeType, map);
        }


        return maps;
    }

    private Map<float> CombineMaps(Map<float> baseMap, Map<float> addMap)
    {
        Map<float> result = new Map<float>(baseMap.Width, baseMap.Height);

        for (int y = 0; y < result.Height; y++)
        {
            for (int x = 0; x < result.Width; x++)
            {
                float baseHeight = baseMap[x, y];
                float biomeHeight = addMap[x, y];
                float finalHeight = baseHeight + biomeHeight * biomeInfluence;

                result[x, y] = finalHeight;
            }
        }

        return result;
    }

    private float GetBiomeMultiplier(EBiome biomeType)
    {
        if (biomeConfigMappings.TryGetValue(biomeType, out var config))
            return config.Generator.heightMultiplier;

        return 1f;
    }

    private Map<float> Normalize(Map<float> map)
    {
        float min = float.MaxValue;
        float max = float.MinValue;

        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                float v = map[x, y];
                if (v < min) min = v;
                if (v > max) max = v;
            }
        }

        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                map[x, y] = Mathf.InverseLerp(min, max, map[x, y]);
            }
        }

        return map;
    }

    private Map<float> BlendBiomeMaps(Map<BiomeWeights> biomeWeightsMap, Dictionary<EBiome, Map<float>> biomeTerrainMaps)
    {
        Map<float> biomeBlendedMap = new Map<float>(biomeWeightsMap.Width, biomeWeightsMap.Height);

        for (int y = 0; y < biomeBlendedMap.Height; y++)
        {
            for (int x = 0; x < biomeBlendedMap.Width; x++)
            {
                BiomeWeights weights = biomeWeightsMap[x, y];
                float finalHeight = 0f;

                //biomeBlendedMap[x, y] = finalHeight;
                //biomeBlendedMap[x, y] = biomeTerrainMaps[b1][x,y]; /// No blending, debug use only.

                //foreach (var biomeMap in biomeTerrainMaps)
                //{
                //    EBiome biome = biomeMap.Key;
                //    float weight = weights.GetWeight(biome);

                //    //Debug.Log($"Biome: {biome}, weight: {weight} " );

                //    if (weight <= 0f)
                //        continue;

                //    float height = biomeMap.Value[x, y] * GetBiomeMultiplier(biome);

                //    if (weight > 0.3f)
                //        finalHeight += height * weight;
                //}

                var (mainBiome, mainWeight) = weights.GetHighestWeight();

                if (mainWeight > 0.9f)
                {
                    biomeBlendedMap[x, y] = biomeTerrainMaps[mainBiome][x, y]
                                            * GetBiomeMultiplier(mainBiome);
                    continue;
                }

                float sharpness = 3f;

                float m = Mathf.Pow(weights.MountainsWeight, sharpness);
                float v = Mathf.Pow(weights.VolcanicWeight, sharpness);
                float d = Mathf.Pow(weights.DesertWeight, sharpness);
                float p = Mathf.Pow(weights.PlainsWeight, sharpness);

                float sum = m + v + d + p;

                m /= sum;
                v /= sum;
                d /= sum;
                p /= sum;

                float minWeight = 0.05f;

                foreach (var kvp in biomeTerrainMaps)
                {
                    EBiome biome = kvp.Key;
                    float weight = 0f;

                    switch (biome)
                    {
                        case EBiome.Mountains: weight = m; break;
                        case EBiome.Volcanic: weight = v; break;
                        case EBiome.Desert: weight = d; break;
                        case EBiome.Plains: weight = p; break;
                    }

                    if (weight < minWeight)
                        continue;

                    float height = kvp.Value[x, y] * GetBiomeMultiplier(biome);
                    finalHeight += height * weight;
                }

                biomeBlendedMap[x, y] = finalHeight;
            }
        }

        return biomeBlendedMap;
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

        for (int z = 0; z < mapHeight; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float vertexHeight = heightMap[x, z] * heightMultiplier;
                vertices[vertexIndex] = new Vector3(topLeftX + x, vertexHeight, topLeftZ - z);

                if (x < mapWidth - 1 && z < mapHeight - 1)
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
                    color = biomeMap[x, y].ToColor();
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
