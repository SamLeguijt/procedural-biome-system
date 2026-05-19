using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlacementResult
{
    public bool IsValid => Prefab != null;

    public GameObject Prefab { get; private set; }
    public Vector3 Position { get; private set; }

    public PlacementResult(GameObject prefab, Vector3 position)
    {
        Prefab = prefab;
        Position = position;
    }
}
