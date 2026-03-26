using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerrainGenerator 
{
    public static Map<float> GenerateTerrainMap(int width, int height, NoiseSettings settings)
    {
        var map = Utils.GenerateNoiseMap(width, height, settings);
        return new Map<float>(map);
    }
}
