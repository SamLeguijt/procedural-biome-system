using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/new DependencyContainer")]
public class DependencyContainer : ScriptableObject
{
    [Header("Adjust World size: ")]
    [SerializeField] private WorldSettings worldSettings;

    [Header("Adjust layout noise maps: ")]
    [SerializeField] private AbstractLayoutGenerator layoutGenerator;

    [Header("Adjust terrain color mode: ")]
    [SerializeField] private BiomeWorldGenerator worldGenerator;

    [Header("Adjust border blending factor: ")]
    [SerializeField] private BaseBiomeAssigner biomeAssigner;

    [Header("Adjust base height and biome height additions: ")]
    [SerializeField] private BaseBiomeTerrainGenerator terrainGenerator;

    [Header("Adjust biome specifics: ")]
    [SerializeField] private BiomeSet biomeSet; 
}
