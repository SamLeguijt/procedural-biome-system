using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeConfig_", menuName ="ScriptableObjects/Biomes/new BiomeConfig")]
public class BiomeConfig : ScriptableObject
{
    [field: SerializeField] public EBiome BiomeType {  get; private set; }
    [field: SerializeField] public NoiseSettings NoiseSettings { get; private set; }
    [field: SerializeField] public float HeightMultiplier { get; private set; }
    [field: SerializeField] public float HeightBaseline {  get; private set; }

    [Space, Header("Terrain sample settings")]
    public Material terrainMaterial;
    public Color debugColor;
    public Vector2 sampleSize = Vector2.one;
    private GameObject recentTerrainSample = null;

    [Button]
    protected void TerrainSample()
    {
        var heightmap = NoiseGenerator.GenerateNoiseMap((int)sampleSize.x, (int)sampleSize.y, NoiseSettings);
        Mesh mesh = MeshGenerator.CreateMesh(heightmap, HeightMultiplier);

        GameObject go = MeshGenerator.CreateGameObjectFromMesh(mesh, terrainMaterial, "TerrainSample");
        recentTerrainSample = go;
    }

    [Button]
    protected void DestroySample()
    {
        if (recentTerrainSample != null)
            DestroyImmediate(recentTerrainSample);
    }
}
