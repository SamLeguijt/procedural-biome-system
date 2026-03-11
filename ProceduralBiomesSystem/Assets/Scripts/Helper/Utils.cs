using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
    public static float GetPerlinNoiseValue(Vector2 input, float frequency, float amplitude, Vector2 offset)
    {
        float x = (input.x * frequency + offset.x);
        float y = (input.y * frequency + offset.y);

        return Mathf.PerlinNoise(x, y) * amplitude;
    }
}
