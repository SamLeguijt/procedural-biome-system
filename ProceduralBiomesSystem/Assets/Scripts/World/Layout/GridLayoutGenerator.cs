using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLayoutGenerator : IWorldLayoutGenerator
{
    public WorldLayout GenerateWorldLayout(WorldSettings settings)
    {
        List<WorldChunk> chunks = new List<WorldChunk>();
        Vector2 gridSize = new Vector2(settings.WorldSize.x / settings.ChunkSize.x, settings.WorldSize.y / settings.ChunkSize.y);
        

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 size = new Vector3(settings.ChunkSize.x, 0, settings.ChunkSize.y);
                Vector3 worldPos = new Vector3(x * settings.ChunkSize.x, 0, y * settings.ChunkSize.y);
                WorldChunk chunk = new WorldChunk(size, worldPos);
                chunks.Add(chunk);
            }
        }

        return new WorldLayout(chunks);
    }
}
