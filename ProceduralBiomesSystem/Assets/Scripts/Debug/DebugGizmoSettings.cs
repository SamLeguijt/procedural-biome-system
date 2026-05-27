using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TerrainDebugVisualiser;

[System.Serializable]
public class DebugGizmoSettings
{
    [Header("Configurations")]
    public DebugMode DebugMode;
    public GizmosType GizmosType;
    public Color GizmoColor;

    [Header("Per GizmoType Settings")]
    public float Radius = 0;
    public Vector3 Size = Vector3.one;
    public Vector3 Direction = Vector3.up;
}

public enum GizmosType
{
    Cube,
    Sphere,
    WireCube,
    WireSphere,
    Ray
}