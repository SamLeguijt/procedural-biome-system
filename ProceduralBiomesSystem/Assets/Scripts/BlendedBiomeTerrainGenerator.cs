using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "BlendedBiomeTerrainGenerator", menuName = "ScriptableObjects/Biomes/new BlendedBiomeTerrainGenerator")]
public class BlendedBiomeTerrainGenerator : BaseBiomeTerrainGenerator
{
    [SerializeField] private BiomeSet biomes;
    [SerializeField, Range(0, 1)] private float minBiomeWeightBlendThreshold;
    [SerializeField, Min(1)] private float biomeMapsInfluence;
    [SerializeField, Min(1)] private float peakSharpness = 5f;
    [SerializeField, Min(1)] private float baseHeightMultiplier = 1;

    public override Map<float> GenerateTerrainMap(Map<float> baseHeightMap, Map<BiomeWeights> biomeWeightsMap)
    {
        var biomeTerrainMaps = GenerateBiomeTerrainMaps(biomeWeightsMap);
        var blendedMap = BlendBiomeMaps(biomeWeightsMap, biomeTerrainMaps);
        var result = CombineMaps(baseHeightMap, blendedMap);

        return blendedMap;
        return result;
    }

    private Map<float> BlendBiomeMaps(Map<BiomeWeights> biomeWeightsMap, Dictionary<BiomeConfig, Map<float>> biomeTerrainMaps)
    {
        Map<float> result = new Map<float>(biomeWeightsMap.Width, biomeWeightsMap.Height);

        for (int y = 0; y < result.Height; y++)
        {
            for (int x = 0; x < result.Width; x++)
            {
                BiomeWeights weights = biomeWeightsMap[x, y];

                float blendedHeight = 0f;
                float totalWeight = 0f;

                foreach (var biomeMapPair in biomeTerrainMaps)
                {
                    BiomeConfig config = biomeMapPair.Key;
                    float weight = weights.GetWeight(config);

                    float influence = Mathf.InverseLerp(minBiomeWeightBlendThreshold, 1f, weight);
                    float biomeHeight = biomeMapPair.Value[x, y];
                    blendedHeight += biomeHeight * influence;
                    totalWeight += influence;


                    //float delta = biomeHeight - config.HeightBaseline;
                    //blendedHeight += config.HeightBaseline * influence + delta * influence;


                    //float falloff = EvaluateFalloff(weight);
                    //float biomeHeight = biomeMapPair.Value[x, y];

                    //// Separate baseline + noise
                    //float baseline = config.HeightBaseline;
                    //float noise = biomeHeight - baseline;

                    //// Apply falloff ONLY to noise
                    //float maskedHeight = baseline + (noise * falloff);

                    //blendedHeight += maskedHeight;
                    //totalWeight += falloff;
                }

                if (totalWeight > 0f)
                    blendedHeight /= totalWeight;

                //float baselineSum = 0f;
                //foreach (var kvp in weights.ConfigWeights)
                //{
                //    BiomeConfig config = kvp.Key;
                //    float weight = kvp.Value;

                //    float influence = Mathf.InverseLerp(minBiomeWeightBlendThreshold, 1f, weight);
                //    baselineSum += config.HeightBaseline * influence;
                //}

                //float baseline = baselineSum / Mathf.Max(totalWeight, 0.0001f);
                //float delta2 = blendedHeight - baseline;

                //float peakInfluence = (delta2 * peakSharpness);
                //float multiplier = CalculateHeightMultiplier(weights);
                //float adjustedDelta = delta2 * Mathf.Lerp(1f, multiplier, peakInfluence);

                //float finalHeight = baseline + adjustedDelta;

                float finalHeight = blendedHeight;

                result[x,y] = finalHeight;
            }
        }

        return result;
    }

    private Dictionary<BiomeConfig, Map<float>> GenerateBiomeTerrainMaps(Map<BiomeWeights> biomeWeightsMap)
    {
        var result = new Dictionary<BiomeConfig, Map<float>>();
        int width = biomeWeightsMap.Width;
        int height = biomeWeightsMap.Height;

        foreach (BiomeConfig config in biomes.Collection)
        {
            var noiseMap = NoiseGenerator.GenerateNoiseMap(width, height, config.NoiseSettings);
            Map<float> mapResult = new Map<float>(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float noiseValue = noiseMap[x, y];
                    float finalHeight = config.HeightBaseline + noiseValue * config.HeightMultiplier;
                    mapResult[x, y] = finalHeight;
                }
            }

            if (!result.ContainsKey(config))
                result.Add(config, mapResult);
        }

        return result;
    }

    private Map<float> CombineMaps(Map<float> baseMap, Map<float> addMap)
    {
        Map<float> result = new Map<float>(baseMap.Width, baseMap.Height);

        for (int y = 0; y < result.Height; y++)
        {
            for (int x = 0; x < result.Width; x++)
            {
                float baseHeight = baseMap[x, y];
                float biomeHeight = addMap[x, y];
                float finalHeight = (baseHeight * baseHeightMultiplier) + (biomeHeight * biomeMapsInfluence);

                result[x, y] = finalHeight;
            }
        }

        return result;
    }

    private float CalculateHeightMultiplier(BiomeWeights weights)
    {
        float multiplierSum = 0f;
        float weightSum = 0f;

        foreach (var kvp in weights.ConfigWeights)
        {
            BiomeConfig config = kvp.Key;
            float weight = weights.GetWeight(kvp.Key);
            multiplierSum += config.HeightMultiplier * weight;
            weightSum += weight;
        }

        return multiplierSum / weightSum;
    }

    private float EvaluateFalloff(float weight)
    {
        float t = Mathf.InverseLerp(minBiomeWeightBlendThreshold, 1f, weight);
        return Mathf.SmoothStep(0f, 1f, t);
    }
}
