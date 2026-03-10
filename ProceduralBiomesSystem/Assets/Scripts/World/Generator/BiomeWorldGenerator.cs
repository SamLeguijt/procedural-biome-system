using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeWorldGenerator : IWorldGenerator
{
    public void GenerateWorld(WorldLayout layout, WorldSettings settings)
    {
        // 1) Assign biomes to the chunks 
        // 2) Generate mesh for each chunk (using WorldChunk.ITerrainGenerator
        // 3) Analyze each chunk, store the info in the chunk 
        // 4) Populate each chunk, store the objects in the chunk
        // 5) Blend biomes (optional)
    }

    private void AssignBiomes(WorldLayout layout)
    {

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
