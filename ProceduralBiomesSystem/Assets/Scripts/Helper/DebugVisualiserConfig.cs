using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TerrainDebugVisualiser;

[CreateAssetMenu(fileName ="DebugGizmoSettings_", menuName = "ScriptableObjects/Debug/new GizmoSettingsConfig")]
public class DebugVisualiserConfig : ScriptableObject
{
    [field: SerializeField] public List<DebugGizmoSettings> GizmoSettings { get; private set; }
}
