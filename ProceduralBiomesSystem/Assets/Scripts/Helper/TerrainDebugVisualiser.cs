using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDebugVisualiser : MonoBehaviour
{
    public enum DebugMode
    {
        None,
        AbsoluteHeights,
        NormalisedLocalHeights,
        Slopes,
    }

    public enum GizmosType
    {
        Cube,
        Sphere,
        WireCube,
        WireSphere,
        Ray
    }

    [System.Serializable]
    public class DebugGizmoSettings
    {
        public DebugMode DebugMode;
        public GizmosType GizmosType;
        public Color GizmoColor;

        [Header("Settings")]
        public float Radius = 0;
        public Vector3 Size = Vector3.one;
        public Vector3 Direction = Vector3.up;
    }

    [field: SerializeField] public DebugMode CurrentMode { get; private set; }
    public List<DebugGizmoSettings> GizmoSettings { get; private set; }

    public float heightAbsoluteThreshold = 100;
    [Range(0, 1)]public float heightNormalisedThreshold = 1;
    public float slopeThreshold;

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

                if (value > heightAbsoluteThreshold)
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

                if (value > slopeThreshold)
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

                if (value > heightNormalisedThreshold)
                {
                    Vector3 worldPosition = GetWorldPosition(x,y,data);
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
