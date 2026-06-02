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
        return ConfigWeights.TryGetValue(config, out var w) ? w : -1f;
    }

    public (BiomeConfig, float) GetHighest()
    {
        BiomeConfig highest = null;
        float max = 0;

        foreach (var kvp in ConfigWeights)
        {
            if (kvp.Value > max)
            {
                max = kvp.Value;
                highest = kvp.Key;
            }
        }

        return (highest, max);
    }

    public List<BiomeConfig> GetBiomes(float minWeight = 0)
    {
        List<BiomeConfig> result = new();

        foreach (var kvp in ConfigWeights)
        {
            if (kvp.Value >= minWeight)
                result.Add(kvp.Key);
        }

        return result;
    }

    public List<BiomeConfig> GetBiomes()
    {
        return ConfigWeights.Keys.ToList();
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
        Color result = Color.black;
        float totalWeight = 0f;

        foreach (var kvp in ConfigWeights)
        {
            BiomeConfig config = kvp.Key;
            float weight = kvp.Value;

            result += config.debugColor * weight;
            totalWeight += weight;
        }

        if (totalWeight > 0f)
            result /= totalWeight;

        return result;
    }
}
