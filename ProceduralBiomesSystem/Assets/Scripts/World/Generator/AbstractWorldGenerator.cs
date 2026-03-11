using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWorldGenerator : ScriptableObject, IWorldGenerator
{
    // TODO: Add default implementation?
    public abstract void GenerateWorld(WorldLayout layout);
}
