using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldSettings_", menuName = "ScriptableObjects/World/new WorldSettings")]
public class WorldSettings : ScriptableObject
{
    [field: Header("World settings")]
    [field: SerializeField] public Vector2Int WorldSize { get; private set; }
    [field: SerializeField] public Vector2Int ChunkQuads {  get; private set; }

}
