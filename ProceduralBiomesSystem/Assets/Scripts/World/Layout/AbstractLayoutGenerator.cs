using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractLayoutGenerator : ScriptableObject, IWorldLayoutGenerator
{
    // TODO: Add default implementation?
    public abstract WorldLayout GenerateWorldLayout(WorldSettings settings);
}
