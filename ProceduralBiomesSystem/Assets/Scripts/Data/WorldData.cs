using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData 
{
    public Mesh Mesh { get; private set; }
    public Material Material { get; private set; }
        
    public Map<float> TerrainMap { get; private set;  }
    public Map<BiomeWeights> BiomeMap { get; private set; }
    public WorldData(Mesh mesh, Material material, Map<float> terrainMap, Map<BiomeWeights> biomeMap = null)
    {
        Mesh = mesh;
        Material = material;
        TerrainMap = terrainMap;
        BiomeMap = biomeMap;
    }
}
