using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeConfig_", menuName ="ScriptableObjects/Biomes/new BiomeConfig")]
public class BiomeConfig : ScriptableObject
{
    // TODO: Is terrain generation now coupled to noise? (abstracted correctly?)
    [field: SerializeField] public BaseTerrainGenerator Generator { get; private set; }

    [Header("Mesh generation:")]
    public Material meshMaterial;
    public float verticeDistance = 1;

    [Range(0, 1)]
    public float NoiseFrequencey = 0.1f;
    public float NoiseAmplitude = 10;
    public bool RandomOffset = true;
    [SerializeField] protected float Offset = 0.01f;

    protected float recentOffset;
    public float NoiseSeed
    {
        get
        {
            if (RandomOffset)
            {
                recentOffset = Random.Range(0, 1000);
                return recentOffset;
            }
            else
            {
                return Offset;
            }
        }
    }

}
