using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData 
{
    public Mesh Mesh { get; private set; }
    public Material Material { get; private set; }

    public WorldData(Mesh mesh, Material material)
    {
        Mesh = mesh;
        Material = material;
    }
}
