using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner 
{
    public GameObject SpawnGameObject(GameObject go, Vector3 at, Quaternion rot, GameObject parent)
    {
        GameObject instantiated = GameObject.Instantiate(go, at, rot, parent.transform);

        return instantiated;
    }
}
