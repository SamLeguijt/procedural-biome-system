using System;
using UnityEngine;

public interface IWorldGenerator 
{
    WorldData GenerateWorld(WorldLayout layout); 
}
