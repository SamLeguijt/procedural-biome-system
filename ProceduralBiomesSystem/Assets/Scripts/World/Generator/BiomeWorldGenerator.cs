using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BiomeWorldGenerator : IWorldGenerator
{
    public void GenerateWorld(WorldLayout layout, WorldSettings settings)
    {

        // 1) Assign biomes to the chunks 
        // 2) Generate mesh for each chunk (using WorldChunk.ITerrainGenerator
        // 3) Analyze each chunk, store the info in the chunk 
        // 4) Populate each chunk, store the objects in the chunk
        // 5) Blend biomes (optional)

        AssignBiomes(layout, settings);
        foreach (WorldChunk chunk in layout.worldChunks)
        {
            CreateMesh(chunk); 
        }
    }

    private void AssignBiomes(WorldLayout layout, WorldSettings settings)
    {
        foreach (WorldChunk chunk in layout.worldChunks)
        {
            chunk.biomeConfig = settings.TempDefaultBiome;
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

    private void CreateMesh(WorldChunk chunk)
    {
        chunk.biomeConfig.Generator.GenerateTerrain(chunk);
    }
}
