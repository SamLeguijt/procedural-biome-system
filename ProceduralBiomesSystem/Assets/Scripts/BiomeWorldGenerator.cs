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
    [Header("Dependencies")]
    [SerializeField] private BaseBiomeAssigner biomeAssigner;
    [SerializeField] private BaseBiomeTerrainGenerator biomeTerrainGenerator;

    [SerializeField] private Material terrainMaterial = null;

    public override WorldData GenerateWorld(WorldLayout layout)
    {
        Map<float> baseHeightMap = layout.ElevationMap;
        Map<BiomeWeights> rawBiomeMap = biomeAssigner.GenerateBiomeMap(layout);
        Map<float> terrainMap = biomeTerrainGenerator.GenerateTerrainMap(baseHeightMap, rawBiomeMap);

        Mesh terrainMesh = MeshGenerator.CreateMesh(terrainMap, 1);
        //Color[] colorMap = BiomeToColorMap(rawBiomeMap, terrainMesh.vertices.Length);

        //terrainMesh.colors = colorMap;

        //Analyze... (in generator ?)
        // Populate... (in generator ?)


        return new WorldData(terrainMesh, terrainMaterial, terrainMap);
    }

    private Color[] BiomeToColorMap(Map<BiomeWeights> biomeMap, int verticesCount)
    {
        Color[] colorMap = new Color[verticesCount];

        int width = biomeMap.Width;
        int height = biomeMap.Height;

        int index = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color finalColor = Color.white;

                if (biomeMap.Contains(x, y))
                {
                    BiomeWeights weights = biomeMap[x, y];

                    // Find dominant biome
                    float maxWeight = 0f;
                    Color dominantColor = Color.white;

                    float totalWeight = 0f;

                    foreach (var kvp in weights.ConfigWeights)
                        totalWeight += kvp.Value;

                    foreach (var kvp in weights.ConfigWeights)
                    {
                        float weight = kvp.Value;
                        if (weight > maxWeight)
                        {
                            maxWeight = weight;
                            dominantColor = kvp.Key.debugColor;
                        }
                    }

                    // Purity = fraction of dominant biome vs all other biomes
                    float purity = maxWeight / Mathf.Max(totalWeight, 0.0001f);
                    // Optional: boost visibility curve
                    float intensity = Mathf.Pow(purity, 1.8f);

                    finalColor = Color.Lerp(Color.white, dominantColor, intensity);
                }

                colorMap[index] = finalColor;
                index++;
            }
        }

        return colorMap;
        //Color[] colorMap = new Color[verticesCount];

        //int width = biomeMap.Width;
        //int height = biomeMap.Height;

        //int index = 0;

        //for (int y = 0; y < height; y++)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        Color finalColor = Color.white;

        //        if (biomeMap.Contains(x, y))
        //        {
        //            BiomeWeights weights = biomeMap[x, y];

        //            float maxWeight = 0f;
        //            Color dominantColor = Color.white;

        //            foreach (var kvp in weights.ConfigWeights)
        //            {
        //                float weight = kvp.Value;

        //                if (weight > maxWeight)
        //                {
        //                    maxWeight = weight;
        //                    dominantColor = kvp.Key.debugColor;
        //                }
        //            }

        //            // Optional: boost visibility
        //            float intensity = Mathf.Pow(maxWeight, 0.5f);

        //            finalColor = Color.Lerp(Color.white, dominantColor, intensity);
        //        }

        //        colorMap[index] = finalColor;
        //        index++;
        //    }
        //}

        //return colorMap;

        /// Debug color intensity blend influence
        //Color[] colorMap = new Color[verticesCount];

        //int width = biomeMap.Width;
        //int height = biomeMap.Height;

        //int index = 0;

        //for (int y = 0; y < height; y++)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        Color finalColor = Color.black;

        //        if (biomeMap.Contains(x, y))
        //        {
        //            BiomeWeights weights = biomeMap[x, y];

        //            foreach (var kvp in weights.ConfigWeights)
        //            {
        //                BiomeConfig config = kvp.Key;
        //                float weight = kvp.Value;

        //                // Optional: boost visibility
        //                weight = Mathf.Pow(weight, 0.5f);

        //                finalColor += config.debugColor * weight;
        //            }
        //        }

        //        // Clamp to valid color range
        //        finalColor.r = Mathf.Clamp01(finalColor.r);
        //        finalColor.g = Mathf.Clamp01(finalColor.g);
        //        finalColor.b = Mathf.Clamp01(finalColor.b);

        //        colorMap[index] = finalColor;
        //        index++;
        //    }
        //}

        //return colorMap;


        /// BIOME COLORS: 
        //Color[] colorMap = new Color[verticesCount];

        //int currentIndex = 0;

        //int width = biomeMap.Width;
        //int height = biomeMap.Height;

        //for (int y = 0; y < height; y++)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        Color color = Color.white;

        //        if (biomeMap.Contains(x, y))
        //        {
        //            color = biomeMap[x, y].ToColor();
        //        }

        //        colorMap[currentIndex] = new Color(color.r, color.g, color.b, color.a);
        //        currentIndex++;
        //    }
        //}

        //return colorMap;
    }

    private void Analyze(WorldChunk chunk)
    {

    }

    private void Populate(WorldChunk chunk)
    {

    }
}
