using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeSet_", menuName = "ScriptableObjects/Biomes/new BiomeSet")]
public class BiomeSet : ScriptableObject
{
    [SerializeField] private List<BiomeConfig> biomesList;

    private HashSet<BiomeConfig> uniqueSet;
    private bool isListDirty = false;

    public HashSet<BiomeConfig> Collection
    {
        get
        {
            if (isListDirty || uniqueSet == null)
            {
                uniqueSet = new HashSet<BiomeConfig>(biomesList);
                isListDirty = false;
            }

            return uniqueSet;
        }
    }

    private void OnValidate()
    {
        isListDirty = true;
    }
}
