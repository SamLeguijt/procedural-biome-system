using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAnalysisData 
{
    public TerrainAnalysisData TerrainData {  get; private set; }

    public WorldAnalysisData(TerrainAnalysisData terrainData)
    {
        TerrainData = terrainData;
    }

    /// Future additions: 
    /// - NavigationData (if implemented)
}
