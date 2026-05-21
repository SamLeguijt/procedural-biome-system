using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldAnalyzer_", menuName = "ScriptableObjects/Analyzation/New WorldAnalyzer")]
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
