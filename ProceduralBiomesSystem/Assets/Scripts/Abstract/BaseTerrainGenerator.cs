using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for generating terrain as a heightmap, 
/// Usefull for generators that only need information about the world layout.
/// </summary>
public abstract class BaseTerrainGenerator : ScriptableObject
{
    public abstract Map<float> GenerateTerrainMap(WorldLayout layout);

}
