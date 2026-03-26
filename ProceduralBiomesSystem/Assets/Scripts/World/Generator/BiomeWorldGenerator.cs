using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[CreateAssetMenu(fileName = "WorldGenerator_", menuName = "ScriptableObjects/World/new WorldGenerator")]
public class BiomeWorldGenerator : AbstractWorldGenerator
{
    [field: SerializeField] public List<BiomeConfig> BiomeConfigs { get; private set; }
    [field: SerializeField] private Material terrainMaterial = null;

    [Header("Blending properties")]
    [Range(0, 10), SerializeField]
    private float biomeInfluence = 1f;

    [Range(0, 1), SerializeField, Tooltip("Determines what minimum weight a point should be in order to be blended")]
    private float minBiomeWeightThreshold = 0.05f;

    public Dictionary<EBiome, Map<float>> recentBiomeMaps;
    private Dictionary<EBiome, BiomeConfig> biomeConfigMappings = new Dictionary<EBiome, BiomeConfig>();

    private void OnValidate()
    {
        if (biomeConfigMappings == null || biomeConfigMappings.Count == 0)
        {
            biomeConfigMappings = new Dictionary<EBiome, BiomeConfig>();

            foreach (BiomeConfig config in BiomeConfigs)
            {
                if (biomeConfigMappings.ContainsKey(config.BiomeType))
                    continue;

                biomeConfigMappings[config.BiomeType] = config;
            }
        }
    }

    public override World GenerateWorld(WorldLayout layout)
    {
        Map<float> baseHeightMap = layout.ElevationMap;

        var biomeHeightMaps = GenerateBiomeTerrainMaps(layout.BiomeMap.Width, layout.BiomeMap.Height);
        var blendedMap = BlendBiomeMaps(layout.BiomeMap, biomeHeightMaps);
        Map<float> finalHeightmap = CombineMaps(baseHeightMap, blendedMap);

        Mesh terrainMesh = MeshGenerator.CreateMesh(finalHeightmap);
        Color[] colorMap = BiomeToColorMap(layout.BiomeMap, terrainMesh.vertices.Length);

        terrainMesh.colors = colorMap;

        // Analyze... (in generator?)
        // Populate... (in generator?)

        // TODO: Store these so we can display them elsewhere?
        //CreateBiomeTerrainMapsDebug(biomeHeightMaps);

        return new World(terrainMesh, terrainMaterial);
    }

    private void CreateBiomeTerrainMapsDebug(Dictionary<EBiome, Map<float>> maps)
    {
        foreach (var kvp in maps)
        {
            var map = kvp.Value;

            Mesh mesh = MeshGenerator.CreateMesh(map);

            GameObject go = new GameObject("Biome terrain debug");
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            MeshFilter filter = go.AddComponent<MeshFilter>();

            filter.sharedMesh = mesh;
            renderer.material = terrainMaterial;
        }
    }

    /// Creates height maps for each biome using their settings.
    private Dictionary<EBiome, Map<float>> GenerateBiomeTerrainMaps(int width, int height)
    {
        var maps = new Dictionary<EBiome, Map<float>>();

        foreach (var config in BiomeConfigs)
        {
            var mapRaw = NoiseGenerator.GenerateNoiseMap(width, height, config.NoiseSettings);
            Map<float> map = new Map<float>(NoiseGenerator.Normalize(mapRaw));
            maps.Add(config.BiomeType, map);
        }

        recentBiomeMaps = maps;
        return maps;
    }

    /// Adds two maps together to produce a final heightmap 
    private Map<float> CombineMaps(Map<float> baseMap, Map<float> addMap)
    {
        Map<float> result = new Map<float>(baseMap.Width, baseMap.Height);

        for (int y = 0; y < result.Height; y++)
        {
            for (int x = 0; x < result.Width; x++)
            {
                float baseHeight = baseMap[x, y];
                float biomeHeight = addMap[x, y];
                float finalHeight = baseHeight + (biomeHeight * biomeInfluence);

                result[x, y] = finalHeight;
            }
        }

        return result;
    }

    /// Gets the BiomeConfig based on enum type
    private BiomeConfig GetBiomeConfig(EBiome biomeType)
    {
        if (biomeConfigMappings.TryGetValue(biomeType, out var config))
            return config;

        foreach (BiomeConfig biomeConfig in BiomeConfigs)
        {
            if (biomeType == biomeConfig.BiomeType)
            {
                biomeConfigMappings[biomeType] = biomeConfig;
                return biomeConfig;
            }
        }

        return null;
    }

    /// Blends all biome maps into a single heightmap using weights per biome map vertex
    private Map<float> BlendBiomeMaps(Map<BiomeWeights> biomeWeightsMap, Dictionary<EBiome, Map<float>> biomeTerrainMaps)
    {
        Map<float> biomeBlendedMap = new Map<float>(biomeWeightsMap.Width, biomeWeightsMap.Height);

        for (int y = 0; y < biomeBlendedMap.Height; y++)
        {
            for (int x = 0; x < biomeBlendedMap.Width; x++)
            {
                BiomeWeights weights = biomeWeightsMap[x, y];

                float blendedHeight = 0f;
                float totalWeight = 0f;

                foreach (var biomeMapPair in biomeTerrainMaps)
                {
                    EBiome biome = biomeMapPair.Key;
                    float weight = weights.GetWeight(biome);

                    float influence = Mathf.InverseLerp(minBiomeWeightThreshold, 1f, weight);
                    blendedHeight += biomeMapPair.Value[x, y] * influence;
                    totalWeight += influence;
                }

                if (totalWeight > 0f)
                    blendedHeight /= totalWeight;

                float baselineSum = 0f;
                foreach (var kvp in weights.WeightMap)
                {
                    BiomeConfig config = GetBiomeConfig(kvp.Key);
                    float weight = kvp.Value;
                    float influence = Mathf.InverseLerp(minBiomeWeightThreshold, 1f, weight);

                    baselineSum += config.HeightBaseline * influence;
                }

                float baseline = baselineSum / Mathf.Max(totalWeight, 0.0001f);

                float delta = blendedHeight - baseline;

                float peakInfluence = Mathf.Clamp01(delta * 5f);
                float multiplier = CalculateHeightMultiplier(weights);
                float adjustedDelta = delta * Mathf.Lerp(1f, multiplier, peakInfluence);

                float finalHeight = baseline + adjustedDelta;

                biomeBlendedMap[x, y] = finalHeight;
            }
        }

        return biomeBlendedMap;
    }

    /// Creates a Color array of the received biome map, setting the veretex colors
    private Color[] BiomeToColorMap(Map<BiomeWeights> biomeMap, int verticesCount)
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

    /// Calculates the heightmultiplier for the blended map based on biome weight on that vertex
    private float CalculateHeightMultiplier(BiomeWeights weights)
    {
        float multiplierSum = 0f;
        float weightSum = 0f;

        foreach (var kvp in weights.WeightMap) 
        {
            BiomeConfig config = GetBiomeConfig(kvp.Key);
            float weight = weights.GetWeight(kvp.Key);
            multiplierSum += config.HeightMultiplier * weight;
            weightSum += weight;
        }

        return multiplierSum / weightSum;
    }

    private void Analyze(WorldChunk chunk)
    {

    }

    private void Populate(WorldChunk chunk)
    {

    }
}
