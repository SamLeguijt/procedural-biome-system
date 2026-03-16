using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

/// <summary>
/// Generates a world of biomes, where each biome is responsible for generating it's own terrain. 
/// </summary>
[CreateAssetMenu(fileName = "WorldGenerator_", menuName = "ScriptableObjects/World/new WorldGenerator")]
public class BiomeWorldGenerator : AbstractWorldGenerator
{
    [field: SerializeField] public List<BiomeSpawnRule> BiomeRules { get; private set; }

    public override void GenerateWorld(WorldLayout layout)
    {
        if (BiomeRules.Count == 0)
            return;

        // 1) Assign biomes to the chunks:
        //AssignBiomes(layout, BiomeRules);

        // 2) Generate mesh for each chunk: [DEPRECATED]
        //foreach (WorldChunk chunk in layout.worldChunks)
        //{
        //chunk.biomeConfig.Generator.GenerateTerrain(chunk);
        //}

        // 3) Analyze each chunk, store the info in the chunk 
        // 4) Populate each chunk, store the objects in the chunk

        /// V2: 
        /// 1) Use biome map to create a heightmap for the mesh 
        /// 2) Use each vertex's biome to sample the position using the generator from that biome
        /// 3) Generate a mesh using the created heightmap
        /// 4) Spawn a gameobject and assign its mesh to it. 

        int width = layout.BiomeMap.Width;
        int height = layout.BiomeMap.Height;
        Map<float> heightMap = new Map<float>(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                EBiome biome = layout.BiomeMap[x, y];
                BiomeConfig config = BiomeRules[0].BiomeConfig;

                foreach (BiomeSpawnRule rule in BiomeRules)
                {
                    if (rule.BiomeConfig.BiomeType == biome)
                        config = rule.BiomeConfig;
                }

                Vector2 worldPos = new Vector2(x, y);
                heightMap[x, y] = config.Generator.GetHeightAtWorldPosition(worldPos);
            }
        }

        Color[] colorMap = CreateColorMap(layout.BiomeMap);
        Mesh terrainMesh = CreateMesh(heightMap.Values, colorMap);

        GameObject world = new GameObject("WORLD");
        MeshFilter meshFilter = world.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = world.AddComponent<MeshRenderer>();

        meshFilter.mesh = terrainMesh;
        meshRenderer.material = BiomeRules[0].BiomeConfig.Generator.MeshMaterial;
    }

    private void AssignBiomes(WorldLayout layout, List<BiomeSpawnRule> settings)
    {
        foreach (WorldChunk chunk in layout.worldChunks)
        {
            BiomeSpawnRule randomSelected = settings[0];

            if (settings.Count > 1)
            {
                float totalWeight = 0f;

                for (int i = 0; i < settings.Count; i++)
                {
                    totalWeight += settings[i].Weight;
                }

                float randomRoll = Random.Range(0f, totalWeight);

                for (int j = 0; j < settings.Count; j++)
                {
                    randomRoll -= settings[j].Weight;

                    if (randomRoll <= 0f)
                    {
                        randomSelected = settings[j];
                        break;
                    }
                }
            }

            chunk.biomeConfig = randomSelected.BiomeConfig;
        }
    }

    private Mesh CreateMesh(float[,] heightMap, Color[] colorMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Vector3[] vertices = new Vector3[width * height];
        Color[] vertexColors = new Color[width * height];
        List<int> triangles = new List<int>();

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int vertexIndex = 0;

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float vertexHeight = heightMap[x, z];
                vertices[vertexIndex] = new Vector3(topLeftX + x, heightMap[x, z], topLeftZ - z);

                if (x < width - 1 && z < height - 1)
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
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.colors = colorMap;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    Color[] CreateColorMap(Map<EBiome> biomeMap)
    {
        int width = biomeMap.Width;
        int height = biomeMap.Height ;

        Color[] colorMap = new Color[width * height];

        int currentIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color color = Color.white;
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
