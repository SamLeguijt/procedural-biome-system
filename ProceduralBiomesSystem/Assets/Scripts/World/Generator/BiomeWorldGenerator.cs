using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

/// <summary>
/// Generates a world of biomes, where each biome generates it's own terrain. 
/// </summary>
[CreateAssetMenu(fileName = "WorldGenerator_", menuName = "ScriptableObjects/World/new WorldGenerator")]
public class BiomeWorldGenerator : AbstractWorldGenerator
{
    [field: SerializeField] public List<BiomeSpawnRule> BiomeRules {  get; private set; }

    public override void GenerateWorld(WorldLayout layout)
    {
        if (BiomeRules.Count == 0)
            return;

        // 1) Assign biomes to the chunks:
        AssignBiomes(layout, BiomeRules);

        // 2) Generate mesh for each chunk:
        foreach (WorldChunk chunk in layout.worldChunks)
        {
            chunk.biomeConfig.Generator.GenerateTerrain(chunk);
        }


        // 3) Analyze each chunk, store the info in the chunk 
        // 4) Populate each chunk, store the objects in the chunk
        // 5) Blend biomes (optional)
    }

    private void AssignBiomes(WorldLayout layout, List<BiomeSpawnRule> settings)
    {
        foreach (WorldChunk chunk in layout.worldChunks)
        {
            chunk.biomeConfig = settings[0].BiomeConfig;
        }
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
