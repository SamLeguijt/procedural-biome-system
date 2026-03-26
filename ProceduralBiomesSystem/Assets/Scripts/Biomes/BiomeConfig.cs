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

    [field: SerializeField] public AbstractMeshTerrainGenerator Generator { get; private set; }
}
