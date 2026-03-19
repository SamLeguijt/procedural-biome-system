using System;
using UnityEngine;

public interface IWorldGenerator 
{
    World GenerateWorld(WorldLayout layout); 
}
