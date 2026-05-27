using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DebugMode
{
    None,
    AbsoluteHeights,
    NormalisedLocalHeights,
    Slopes,
    BiomePurity
}

public class TerrainDebugVisualiser : MonoBehaviour
{
    [field: SerializeField] public DebugMode CurrentMode { get; private set; }
    public List<DebugGizmoSettings> GizmoSettings { get; private set; }

    public ValueRange absoluteHeightRange;
    public ValueRangeNormalised normalizedLocalHeightRange;
    public ValueRangeNormalised slopeRange;
    public ValueRangeNormalised biomePurtiyRange;

    private WorldAnalysisData analysisData;

    public void SetWorldAnalysisData(WorldAnalysisData data, DebugVisualiserConfig config)
    {
        analysisData = data;
        GizmoSettings = config.GizmoSettings;
    }

    public void OnDrawGizmosSelected()
    {
        if (analysisData == null)
            return;

        switch (CurrentMode)
        {
            case DebugMode.None:
                return;
            case DebugMode.AbsoluteHeights:
                DrawAbsoluteHeightGizmos(analysisData);
                break;
            case DebugMode.Slopes:
                DrawSlopeGizmos(analysisData);
                break;
            case DebugMode.NormalisedLocalHeights:
                DrawNormalisedHeightGizmos(analysisData);
                break;
            case DebugMode.BiomePurity:
                DrawBiomePurityGizmos(analysisData);
                break;
        }
    }

    private void DrawBiomePurityGizmos(WorldAnalysisData analysisData)
    {
        Map<BiomeWeights> biomeWeights = analysisData.TerrainData.BiomeWeightsMap;
        DebugGizmoSettings mapping = GetMapping(CurrentMode);

        if (mapping == null)
            return;

        for (int y = 0; y < biomeWeights.Height; y++)
        {
            for (int x = 0; x < biomeWeights.Width; x++)
            {
                var highest = biomeWeights[x,y].GetHighest();

                if (biomePurtiyRange.FallsInRange(highest.Item2))
                {
                    Vector3 worldPosition = GetWorldPosition(x, y, analysisData);
                    DrawGizmoFromSettings(worldPosition, mapping);
                }
            }
        }
    }

    private void DrawAbsoluteHeightGizmos(WorldAnalysisData data)
    {
        Map<float> heights = data.TerrainData.HeightMap;
        DebugGizmoSettings mapping = GetMapping(CurrentMode);

        if (mapping == null)
            return;

        for (int y = 0; y < heights.Height; y++)
        {
            for (int x = 0; x < heights.Width; x++)
            {
                float value = heights[x, y];
                Vector3 worldPosition = GetWorldPosition(x, y, data);

                if (absoluteHeightRange.FallsInRange(value))
                {
                    DrawGizmoFromSettings(worldPosition, mapping);
                }
            }
        }
    }

    private void DrawSlopeGizmos(WorldAnalysisData data)
    {
        Map<float> slopes = data.TerrainData.SlopeMap;
        DebugGizmoSettings mapping = GetMapping(CurrentMode);

        if (mapping == null)
            return;

        for (int y = 0; y < slopes.Height; y++)
        {
            for (int x = 0; x < slopes.Width; x++)
            {
                float value = slopes[x, y];
                Vector3 worldPosition = GetWorldPosition(x, y, data);

                if (slopeRange.FallsInRange(value))
                {
                    DrawGizmoFromSettings(worldPosition, mapping);
                }
            }
        }
    }

    private void DrawNormalisedHeightGizmos(WorldAnalysisData data)
    {
        Map<float> heights = data.TerrainData.HeightMap;
        DebugGizmoSettings mapping = GetMapping(CurrentMode);

        for (int y = 0; y < heights.Height; y++)
        {
            for (int x = 0; x < heights.Width; x++)
            {
                BiomeConfig primaryBiome = data.TerrainData.GetPrimaryBiome(x, y);
                float value = data.TerrainData.GetNormalisedHeight(x, y, primaryBiome);

                if (normalizedLocalHeightRange.FallsInRange(value))
                {
                    Vector3 worldPosition = GetWorldPosition(x, y, data);
                    DrawGizmoFromSettings(worldPosition, mapping);
                }
            }
        }
    }

    private void DrawGizmoFromSettings(Vector3 position, DebugGizmoSettings mapping)
    {
        Gizmos.color = mapping.GizmoColor;

        switch (mapping.GizmosType)
        {
            case GizmosType.Cube:
                Gizmos.DrawCube(position, mapping.Size);
                break;
            case GizmosType.Sphere:
                Gizmos.DrawSphere(position, mapping.Radius);
                break;
            case GizmosType.WireCube:
                Gizmos.DrawWireCube(position, mapping.Size);
                break;
            case GizmosType.WireSphere:
                Gizmos.DrawWireSphere(position, mapping.Radius);
                break;
            case GizmosType.Ray:
                Gizmos.DrawRay(position, mapping.Direction);
                break;
        }
    }

    private Vector3 GetWorldPosition(int x, int y, WorldAnalysisData data)
    {
        Map<float> heightMap = data.TerrainData.HeightMap;
        float heightValue = data.TerrainData.GetAbsoluteHeight(x, y);
        Vector3 objectPosition = transform.position;

        return objectPosition + TerrainSpaceUtils.GridToTerrainWorld(x, y, heightMap.Width, heightMap.Height, heightValue);
    }

    private DebugGizmoSettings GetMapping(DebugMode mode)
    {
        if (GizmoSettings == null || GizmoSettings.Count == 0)
            return null;

        foreach (DebugGizmoSettings map in GizmoSettings)
        {
            if (map.DebugMode == mode)
                return map;
        }

        return null;
    }
}
