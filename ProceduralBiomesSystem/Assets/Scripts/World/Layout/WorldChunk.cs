using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldChunk 
{
    public Vector3 Size { get; private set; }
    public Vector3 WorldPosition { get; private set; }

    public Mesh mesh = null;
    public BiomeConfig biomeConfig = null;

    public WorldChunk(Vector3 size, Vector3 worldPos)
    {
        Size = size;
        WorldPosition = worldPos;
    }
}
