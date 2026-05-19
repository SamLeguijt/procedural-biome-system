using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeConfig_", menuName ="ScriptableObjects/Biomes/new BiomeConfig")]
public class BiomeConfig : ScriptableObject
{
    [field: Header("Terrain settings")]
    [field: SerializeField] public NoiseSettings NoiseSettings { get; private set; }
    [field: SerializeField] public float HeightMultiplier { get; private set; }
    [field: SerializeField] public float HeightBaseline {  get; private set; }
    [field: SerializeField] public Color debugColor {  get; private set; }

    [field: Space, Header("Biome rules")]
    [field: SerializeField] public List<AbstractBiomeRule> BiomeRules { get; private set; }

    [field: Space, Header("Population rules")]
    [field: SerializeField] public List<PlacementRule> PopulationRules { get; private set; }

    [Space, Header("Terrain sample settings")]
    public Material terrainMaterial;
    public Vector2 sampleSize = Vector2.one;
    private GameObject recentTerrainSample = null;

    [Button]
    protected void TerrainSample()
    {
        var heightmap = NoiseGenerator.GenerateNoiseMap((int)sampleSize.x, (int)sampleSize.y, NoiseSettings);
        Mesh mesh = MeshGenerator.CreateMesh(heightmap, HeightMultiplier);
        ApplyDebugColorToMesh(mesh);
        GameObject go = MeshGenerator.CreateGameObjectFromMesh(mesh, terrainMaterial, "TerrainSample");
        recentTerrainSample = go;
    }

    [Button]
    protected void DestroySample()
    {
        if (recentTerrainSample != null)
            DestroyImmediate(recentTerrainSample);
    }

    private void ApplyDebugColorToMesh(Mesh mesh)
    {
        Color[] colors = new Color[mesh.vertexCount];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = debugColor;
        }

        mesh.colors = colors;
    }

    private void OnValidate()
    {
        if (recentTerrainSample != null)
        {
            var heightmap = NoiseGenerator.GenerateNoiseMap((int)sampleSize.x, (int)sampleSize.y, NoiseSettings);
            Mesh mesh = MeshGenerator.CreateMesh(heightmap, HeightMultiplier);
            ApplyDebugColorToMesh(mesh);

            recentTerrainSample.GetComponent<MeshFilter>().mesh = mesh;
        }
    }
}
