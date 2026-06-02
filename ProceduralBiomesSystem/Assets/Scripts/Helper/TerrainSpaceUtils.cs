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

    public static Vector2Int TerrainWorldToGrid(Vector3 worldPos, int width, int height)
    {
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int x = Mathf.RoundToInt(worldPos.x - topLeftX);
        int y = Mathf.RoundToInt(topLeftZ - worldPos.z);

        return new Vector2Int(x, y);
    }
}
