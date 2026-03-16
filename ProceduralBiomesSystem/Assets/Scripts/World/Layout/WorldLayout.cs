using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLayout 
{
    public int Width; 
    public int Height;
    public Map<EBiome> BiomeMap;

    public List<WorldChunk> worldChunks;
    
    public WorldLayout(Map<EBiome> biomeMap)
    {
        BiomeMap = biomeMap;
        this.Width = BiomeMap.Width;
        this.Height = BiomeMap.Height;
    }

    public WorldLayout(List<WorldChunk> chunks)
    {
        worldChunks = chunks;
    }
}
