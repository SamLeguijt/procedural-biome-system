using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rename to AnalysisResult
// Should contain:
// - Terrain information 
// - (Height maps, slope maps
public class WorldAnalysisData 
{
    public TerrainAnalysisData TerrainData {  get; private set; }
    public List<PopulationCandidate> GeneratedCandidates { get; set; }


    public WorldAnalysisData(TerrainAnalysisData terrainData)
    {
        TerrainData = terrainData;
    }

    /// Future additions: 
    /// - NavigationData (if implemented)
}
