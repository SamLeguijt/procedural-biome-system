using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LowHeightHighTempRule", menuName = "ScriptableObjects/Biomes/Rules/new low-height, high-temp rule")]
public class LowWarmLandRule : ABiomeLocationRule
{
    public override float Evaluate(BiomeConfig config, float elevationValue, float humidityValue, float erosionValue, float temperatureValue)
    {
        float weight = (1f - elevationValue) * temperatureValue;
        return weight;
    }
}
