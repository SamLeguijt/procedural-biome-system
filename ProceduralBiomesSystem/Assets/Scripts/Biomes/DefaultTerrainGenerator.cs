using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TerrainGenerator_Default", menuName = "ScriptableObjects/Terrain/new Default generator")]
public class DefaultTerrainGenerator : AbstractMeshTerrainGenerator
{
    public override void GenerateTerrain(WorldChunk chunk)
    {
        var heightMap = GenerateHeightMap(chunk);
        Mesh mesh = CreateMesh(heightMap);
        chunk.mesh = mesh;
    }
}
