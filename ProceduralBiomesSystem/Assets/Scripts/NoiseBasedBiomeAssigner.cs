using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NoiseBased_BiomeAssigner", menuName = "ScriptableObjects/Biomes/new NoiseBasedBiomeAssigner")]
public class NoiseBasedBiomeAssigner : ABiomeAssigner
{
    [SerializeField] private float blendFactor = 1;

    public override Map<BiomeWeights> GenerateBiomeInfluenceMap(WorldLayout layout, HashSet<BiomeConfig> possibleBiomes)
    {
        Map<float> elevationMap = layout.ElevationMap;
        Map<float> erosionMap = layout.ErosionMap;
        Map<float> humidityMap = layout.HumidityMap;
        Map<float> temperatureMap = layout.TemperatureMap;

        int width = elevationMap.Width;
        int height = elevationMap.Height;

        Map<BiomeWeights> biomeMap = new Map<BiomeWeights>(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float elevationValue = elevationMap[x, y];
                float erosionValue = erosionMap[x, y];
                float humidityValue = humidityMap[x, y];
                float temperatureValue = temperatureMap[x, y];
                Dictionary<BiomeConfig, float> weights = new Dictionary<BiomeConfig, float>();

                foreach (var config in possibleBiomes)
                {
                    float weight = 0f;
                    foreach (ABiomeLocationRule rule in config.BiomeRules)
                    {
                        /// TODO: Turn this into some context Dictionary that maps string to Map<float> instead.
                        weight += Mathf.Max(0f, rule.Evaluate(config, elevationValue, humidityValue, erosionValue, temperatureValue));
                    }

                    weight = Mathf.Pow(weight, blendFactor); 
                    weights[config] = weight;


                }

                float totalWeight = 0f;

                foreach (var kvp in weights)
                    totalWeight += kvp.Value;

                foreach (var key in weights.Keys.ToList())
                {
                    weights[key] /= Mathf.Max(totalWeight, 0.0001f);
                }

                biomeMap[x, y] = new BiomeWeights(weights);
            }
        }

        return biomeMap;
    }
}
