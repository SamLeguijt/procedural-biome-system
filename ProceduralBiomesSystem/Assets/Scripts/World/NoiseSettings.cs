using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NoiseSettings
{
    [Header("Noise Settings")]
    public bool useRandomSeed; 
    public int seed;

    [Space, Min(1)] public int octaves;
    [Min(0.0001f)]public float persistance;
    [Min(0.0001f)]public float lacunarity;

    [Space, Min(0.001f)] public float scale;
    public Vector2 offset;
    //public int heightMultiplier;

    [Space] public bool useCurve;
    public AnimationCurve remapCurve;

    public bool applyNormalise;
}
