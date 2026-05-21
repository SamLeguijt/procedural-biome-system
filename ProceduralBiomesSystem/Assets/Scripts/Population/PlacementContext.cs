using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementContext 
{
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
