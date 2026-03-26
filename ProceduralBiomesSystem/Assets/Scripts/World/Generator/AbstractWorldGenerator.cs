using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWorldGenerator : ScriptableObject, IWorldGenerator
{
    public abstract World GenerateWorld(WorldLayout layout);
}
