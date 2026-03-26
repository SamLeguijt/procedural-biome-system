using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeSet_", menuName = "ScriptableObjects/Biomes/new BiomeSet")]
public class BiomeSet : ScriptableObject
{
    [field: SerializeField] public List<BiomeConfig> Collection {  get; private set; } 
}
