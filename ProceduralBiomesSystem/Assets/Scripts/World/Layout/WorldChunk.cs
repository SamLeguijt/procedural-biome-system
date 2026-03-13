using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldChunk 
{
    public Vector2Int Quads { get; private set; }
    public Vector3 WorldPosition { get; private set; }

    public Mesh mesh = null;
    public BiomeConfig biomeConfig = null;

    public WorldChunk(Vector2Int quads, Vector3 worldPos)
    {
        Quads = quads;
        WorldPosition = worldPos;
    }
}
