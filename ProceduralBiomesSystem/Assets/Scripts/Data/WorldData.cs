using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: This class should be the final result of the whole generation pipeline
// So, containing ALL relevant information and data 
// Refactor: Internal pipeline usage (debuggers, visualisers and other should be using this)
public class WorldData 
{
    public Mesh Mesh { get; private set; }
    public Material Material { get; private set; }
    public TerrainData TerrainData { get; private set; }
    public Map<BiomeWeights> BiomeMap { get; private set; }

    public WorldData(Mesh mesh, Material material, TerrainData terrainData, Map<BiomeWeights> biomeMap = null)
    {
        Mesh = mesh;
        Material = material;
        TerrainData = terrainData;
        BiomeMap = biomeMap;
    }
}
