using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLayout 
{
    public List<WorldChunk> worldChunks;
    
    public WorldLayout(List<WorldChunk> chunks)
    {
        worldChunks = chunks;
    }
}
