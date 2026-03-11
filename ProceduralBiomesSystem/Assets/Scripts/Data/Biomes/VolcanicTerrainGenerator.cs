using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainGenerator_", menuName = "ScriptableObjects/Terrain/new Volcanic Terrain generator")]
public class VolcanicTerrainGenerator : MeshTerrainGenerator
{
    public override void GenerateTerrain(WorldChunk chunk)
    {
        CreateMesh(chunk);
    }
}
