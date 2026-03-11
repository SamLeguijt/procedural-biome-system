using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LayoutGenerator_", menuName = "ScriptableObjects/World/new LayoutGenerator")]
public class GridLayoutGenerator : AbstractLayoutGenerator
{
    public override WorldLayout GenerateWorldLayout(WorldSettings settings)
    {
        List<WorldChunk> chunks = new List<WorldChunk>();
        Vector2 gridSize = new Vector2(settings.WorldSize.x / settings.ChunkSize.x, settings.WorldSize.y / settings.ChunkSize.y);
        

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 size = new Vector3(settings.ChunkSize.x, 0, settings.ChunkSize.y);
                Vector3 worldPos = new Vector3(x * settings.ChunkSize.x + settings.ChunkSize.x / 2f, 0, y * settings.ChunkSize.y + settings.ChunkSize.y / 2f);
                WorldChunk chunk = new WorldChunk(size, worldPos);
                chunks.Add(chunk);
            }
        }

        return new WorldLayout(chunks);
    }
}
