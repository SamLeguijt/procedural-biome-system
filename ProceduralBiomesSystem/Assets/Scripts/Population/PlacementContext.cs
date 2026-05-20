using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementContext 
{
    /// TODO: 
    /// This class should be used to store information about a given point on the terrain. 
    /// 

    // Coordinates
    public int X {  get; private set; }
    public int Y {  get; private set; }
    public TerrainAnalysisData AnalyseData { get; private set; }
}
