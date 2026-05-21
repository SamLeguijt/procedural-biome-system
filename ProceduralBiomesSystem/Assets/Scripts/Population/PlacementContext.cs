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
    public Vector3 WorldPosition { get; private set; }
    public TerrainAnalysisData AnalyseData { get; private set; }

    public PlacementContext(int x, int y, Vector3 worldPosition, TerrainAnalysisData analyseData)
    {
        X = x;
        Y = y;
        WorldPosition = worldPosition;
        AnalyseData = analyseData;
    }
}
