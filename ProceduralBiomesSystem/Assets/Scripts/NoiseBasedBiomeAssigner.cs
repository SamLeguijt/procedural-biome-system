using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoiseBased_BiomeAssigner", menuName = "ScriptableObjects/Biomes/new NoiseBasedBiomeAssigner")]
public class NoiseBasedBiomeAssigner : BaseBiomeAssigner
{
    [SerializeField] private float blendFactor = 1;

    public override Map<BiomeWeights> GenerateBiomeMap(WorldLayout layout)
    {
        Map<float> elevationMap = layout.ElevationMap;
        Map<float> erosionMap = layout.ErosionMap;
        Map<float> humidityMap = layout.HumidityMap;
        Map<float> temperatureMap = layout.TemperatureMap;

        int width = elevationMap.Width;
        int height = elevationMap.Height;

        Map<BiomeWeights> biomeMap = new Map<BiomeWeights>(width, height);
        Dictionary<BiomeConfig, float> weights = new Dictionary<BiomeConfig, float>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float elevationValue = elevationMap[x, y];
                float erosionValue = erosionMap[x, y];
                float humidityValue = humidityMap[x, y];
                float temperatureValue = temperatureMap[x, y];

                foreach (var config in BiomeSet.Collection)
                {
                    float weight = 0f;
                    foreach (AbstractBiomeRule rule in config.BiomeRules)
                    {
                        /// TODO: Turn this into some context Dictionary that maps string to Map<float> instead.
                        weight += rule.Evaluate(config, elevationValue, humidityValue, erosionValue, temperatureValue);
                    }

                    weight = Mathf.Pow(weight, blendFactor); 
                    weights[config] = weight;
                }



                biomeMap[x, y] = new BiomeWeights(weights);
            }
        }

        return biomeMap;
    }
}
