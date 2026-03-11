using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainGenerator_Desert", menuName = "ScriptableObjects/Terrain/new Desert generator")]
public class DesertTerrainGenerator : AbstractMeshTerrainGenerator
{
    public override void GenerateTerrain(WorldChunk chunk)
    {
        CreateMesh(chunk);
    }
}
