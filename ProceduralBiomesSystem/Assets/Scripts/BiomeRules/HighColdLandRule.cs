using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HighHeightLowTempRule", menuName = "ScriptableObjects/Biomes/Rules/new high-height, low-temp rule")]
public class HighColdLandRule : ABiomeLocationRule
{
    public override float Evaluate(BiomeConfig config, float elevationValue, float humidityValue, float erosionValue, float temperatureValue)
    {
        float weight = elevationValue * (1f - temperatureValue);
        return weight;
    }
}
