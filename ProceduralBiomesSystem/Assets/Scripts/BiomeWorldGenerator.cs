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

        Mesh terrainMesh = MeshGenerator.CreateMesh(terrainMap);
        Color[] colorMap = BiomeToColorMap(rawBiomeMap, terrainMesh.vertices.Length);

        terrainMesh.colors = colorMap;

        //Analyze... (in generator ?)
        // Populate... (in generator ?)


        return new WorldData(terrainMesh, terrainMaterial, terrainMap);
    }

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

    private void Analyze(WorldChunk chunk)
    {

    }

    private void Populate(WorldChunk chunk)
    {

    }
}
