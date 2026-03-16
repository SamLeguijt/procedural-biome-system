using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BiomeRule_", menuName ="ScriptableObjects/Biomes/new BiomeSpawnRule")]
public class BiomeSpawnRule : ScriptableObject
{
    [field: SerializeField] public BiomeConfig BiomeConfig { get; private set; }
    [field: SerializeField] public float Weight { get; private set; }
}
