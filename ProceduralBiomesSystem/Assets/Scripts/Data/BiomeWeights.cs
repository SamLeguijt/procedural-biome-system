using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class BiomeWeights
{
    public Dictionary<BiomeConfig, float> ConfigWeights { get; private set; } = new Dictionary<BiomeConfig, float>();

    public BiomeWeights(Dictionary<BiomeConfig, float> weights)
    {
        ConfigWeights = new Dictionary<BiomeConfig, float>(weights);
        Normalise();
    }

    public float GetWeight(BiomeConfig config)
    {
        return ConfigWeights.TryGetValue(config, out var w) ? w : 0f;
    }

    private void Normalise()
    {
        float sum = ConfigWeights.Values.Sum();
        if (sum > 0f)
        {
            var keys = ConfigWeights.Keys.ToList();
            foreach (var key in keys)
                ConfigWeights[key] /= sum;
        }
    }

    public Color ToColor()
    {
        var color = new Color();
        return color;
        //return new Color(DesertWeight, MountainsWeight, VolcanicWeight, PlainsWeight); 
    }
}
