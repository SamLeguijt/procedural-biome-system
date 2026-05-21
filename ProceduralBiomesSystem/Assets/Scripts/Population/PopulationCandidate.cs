using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationCandidate 
{
    public GameObject Prefab {  get; private set; }
    public Vector3 WorldPosition {  get; private set; }
    public Quaternion Rotation {  get; private set; }

    public float OccupationRadius {  get; private set; } 


    public PopulationCandidate(GameObject prefab, Vector3 worldPos, Quaternion rotation, float radius)
    {
        this.Prefab = prefab;
        this.WorldPosition = worldPos;
        this.Rotation = rotation;
        this.OccupationRadius = radius;
    }
}
