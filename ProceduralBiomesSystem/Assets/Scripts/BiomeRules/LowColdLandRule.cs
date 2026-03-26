using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LowHeightLowTempRule", menuName = "ScriptableObjects/Biomes/Rules/new low-height, low-temp rule")]
public class LowColdLandRule : AbstractBiomeRule
{
    public override float Evaluate(BiomeConfig config, float elevationValue, float humidityValue, float erosionValue, float temperatureValue)
    {
        float weight = (1f - elevationValue) * (1f - temperatureValue);
        return weight;
    }
}
