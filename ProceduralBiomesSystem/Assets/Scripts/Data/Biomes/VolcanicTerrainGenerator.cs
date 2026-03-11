using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainGenerator_Volcanic", menuName = "ScriptableObjects/Terrain/new Volcanic generator")]
public class VolcanicTerrainGenerator : AbstractMeshTerrainGenerator
{
    public override void GenerateTerrain(WorldChunk chunk)
    {
        CreateMesh(chunk);
    }
}
