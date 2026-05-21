using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerrainSpaceUtils 
{
    public static Vector3 GridToTerrainWorld(int x, int y, int width, int height, float heightValue)
    {
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        return new Vector3
        (
            topLeftX + x,
            heightValue,
            topLeftZ - y
        );
    }
}
