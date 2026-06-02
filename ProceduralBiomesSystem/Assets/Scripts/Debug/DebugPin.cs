using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPin : MonoBehaviour
{
    private WorldAnalysisData analysisData;


    public void SetData(WorldAnalysisData analysisData)
    {
        this.analysisData = analysisData;
    }

    [Button]
    private void LogInfo()
    {
        Vector2Int coord = GetMapCoordinates();
        if (coord == new Vector2Int(-1, -1))
            Debug.Log("No info available for this coordinate");

        string positionInfo = $"World Pos: {transform.position}";
        string coordInfo = $"Map Coordinate: {coord}";
        
        var biomeMap = analysisData.TerrainData.BiomeWeightsMap;
        var heightMap = analysisData.TerrainData.HeightMap;
        var weights = biomeMap[coord.x, coord.y];

        string weightInfo = string.Empty;

        foreach (var kvp in weights.ConfigWeights)
        {
            weightInfo += $" - {kvp.Key.name}: {kvp.Value:F4}";
        }

        string heightInfo = $"Final Height: {heightMap[coord.x, coord.y]:F4}";
       
        Debug.Log($"{coordInfo} - {positionInfo} \n {weightInfo} --- {heightInfo}");
    }


    [Button]
    void SnapToTerrain()
    {
        var heightmap = analysisData.TerrainData.HeightMap;

        Vector3 position = transform.position;
        Vector2Int mapCoordinates = TerrainSpaceUtils.TerrainWorldToGrid(position, heightmap.Width, heightmap.Height);

        if (GetMapCoordinates() != new Vector2Int(-1,-1))
        {
            float height = heightmap[mapCoordinates.x, mapCoordinates.y];
            transform.position = new Vector3(position.x, height, position.z);
        }
    }

    private Vector2Int GetMapCoordinates()
    {
        if (analysisData == null)
            return new Vector2Int(-1, -1);

        var heightmap = analysisData.TerrainData.HeightMap;

        Vector3 position = transform.position;
        Vector2Int mapCoordinates = TerrainSpaceUtils.TerrainWorldToGrid(position, heightmap.Width, heightmap.Height);

        if (heightmap.Contains(mapCoordinates.x, mapCoordinates.y))
        {
            return mapCoordinates;
        }

        return new Vector2Int(-1,-1);
    }

    private Vector3 GetTerrainPosition()
    {
        if (analysisData == null)
            return Vector3.zero;

        Vector2Int coord = GetMapCoordinates();

        var heightmap = analysisData.TerrainData.HeightMap;

        if (coord != new Vector2Int(-1, -1))
        {
            float height = heightmap[coord.x, coord.y];

            return new Vector3(transform.position.x, height, transform.position.z);
        }

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Vector2Int coord = GetMapCoordinates();
        float distance = Vector3.Distance(transform.position, GetTerrainPosition());

        if (coord == new Vector2Int(-1, -1))
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        Gizmos.DrawRay(transform.position, -transform.up * distance);
    }
}
