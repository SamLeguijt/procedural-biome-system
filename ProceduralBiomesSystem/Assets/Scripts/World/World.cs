using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World 
{
    public Mesh Mesh { get; private set; }
    public Material Material { get; private set; }
    // Mesh per chunk(s)?

    public World(Mesh mesh, Material material)
    {
        Mesh = mesh;
        Material = material;
    }
}
