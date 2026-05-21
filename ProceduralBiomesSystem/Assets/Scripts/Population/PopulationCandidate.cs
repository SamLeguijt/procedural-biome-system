using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents a possible candidate to procedurally place. 
// Gets created by the population rules, resolved by a populate strategy to decide final PopulateInstances.
// Todo: Seperate PopulateCandidate and PopulateInstance classes?
public class PopulationCandidate 
{
    public GameObject prefab;
    public Vector3 worldPos; 
    public Quaternion rotation;

    public float radius; 

    public PopulationCandidate(GameObject prefab, Vector3 worldPos, Quaternion rotation, float radius = 0)
    {
        this.prefab = prefab;
        this.worldPos = worldPos;
        this.rotation = rotation;
        this.radius = radius;
    }
}
