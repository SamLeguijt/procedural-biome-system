using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAnalyzer : ScriptableObject
{
    [field: SerializeField] public TerrainAnalyzer TerrainAnalyzer { get; private set; }

    // Future additions:
    // - NavigationAnalyzer
    // - Biomes? 


    public WorldAnalysisData GetAnalysis(WorldData data)
    {
        TerrainAnalysisData terrainData = TerrainAnalyzer.AnalyzeTerrain(data);

        return new WorldAnalysisData(terrainData);
    }
}
