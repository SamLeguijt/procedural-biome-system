using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HighHeightHighTempRule", menuName = "ScriptableObjects/Biomes/Rules/new high-height, high-temp rule")]
public class HighWarmLandRule : ABiomeLocationRule
{
    public override float Evaluate(BiomeConfig config, float elevationValue, float humidityValue, float erosionValue, float temperatureValue)
    {
        float weight = elevationValue * temperatureValue;
        return weight;
    }
}
