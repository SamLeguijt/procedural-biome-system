using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoiseBased_BiomeAssigner", menuName = "ScriptableObjects/Biomes/new NoiseBasedBiomeAssigner")]
public class NoiseBasedBiomeAssigner : BaseBiomeAssigner
{
    [SerializeField] private float sharpness = 1;

    public override Map<BiomeWeights> GenerateBiomeMap(WorldLayout layout)
    {
        Map<float> elevationMap = layout.ElevationMap;
        Map<float> erosionMap = layout.ErosionMap;
        Map<float> humidityMap = layout.HumidityMap;

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

                foreach (var config in BiomeSet.Collection)
                {
                    float weight = 0f;

                    switch (config.BiomeType)
                    {
                        case EBiome.Desert:
                            weight = (1f - humidityValue) * (1f - elevationValue);
                            break;

                        case EBiome.Mountains:
                            weight = elevationValue * (1f - erosionValue);
                            break;

                        case EBiome.Plains:
                            weight = (1f - elevationValue) * humidityValue;
                            break;

                        case EBiome.Volcanic:
                            weight = elevationValue * erosionValue;
                            break;
                    }

                    weight = Mathf.Pow(weight, sharpness);
                    weights[config] = weight;
                }

                biomeMap[x, y] = new BiomeWeights(weights);
            }
        }

        return biomeMap;
    }
}
