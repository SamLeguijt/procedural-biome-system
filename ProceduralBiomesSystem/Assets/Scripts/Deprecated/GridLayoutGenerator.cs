using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LayoutGenerator_", menuName = "ScriptableObjects/World/new LayoutGenerator")]
public class GridLayoutGenerator : ALayoutGenerator
{
    public override WorldLayout GenerateWorldLayout(WorldSettings settings)
    {
        List<WorldChunk> chunks = new List<WorldChunk>();
        Vector2 gridSize = new Vector2(settings.WorldSize.x / settings.ChunkQuads.x, settings.WorldSize.y / settings.ChunkQuads.y);
        
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int quads = new Vector2Int(settings.ChunkQuads.x, settings.ChunkQuads.y);
                Vector3 worldPos = new Vector3(x * settings.ChunkQuads.x + settings.ChunkQuads.x / 2f, 0, y * settings.ChunkQuads.y + settings.ChunkQuads.y / 2f);
                WorldChunk chunk = new WorldChunk(quads, worldPos);
                chunks.Add(chunk);
            }
        }

        return new WorldLayout(chunks);
    }
}
