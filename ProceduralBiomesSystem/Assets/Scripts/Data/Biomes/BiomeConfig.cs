using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeConfig_", menuName ="ScriptableObjects/Biomes/new BiomeConfig")]
public class BiomeConfig : ScriptableObject
{
    // TODO: Is terrain generation now coupled to noise? (abstracted correctly?)
    [field: SerializeField] public AbstractMeshTerrainGenerator Generator { get; private set; }



}
