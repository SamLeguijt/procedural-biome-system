using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NoiseSettings
{
    [Header("Noise Settings")]
    public bool useRandomSeed; 
    public int seed;

    [Space] public int octaves;
    public float persistance;
    public float lacunarity;

    [Space] public float scale;
    public Vector2 offset;
    public int heightMultiplier;

    [Space] public bool useCurve;
    public AnimationCurve remapCurve; 
}
