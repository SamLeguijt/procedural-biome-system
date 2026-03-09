using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTerrainGenerator : ScriptableObject, ITerrainGenerator
{
    public abstract void GenerateTerrain(WorldChunk chunk);
}
