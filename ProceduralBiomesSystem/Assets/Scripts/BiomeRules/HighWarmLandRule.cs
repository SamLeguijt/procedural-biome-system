using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HighHeightHighTempRule", menuName = "ScriptableObjects/Biomes/Rules/new high-height, high-temp rule")]
public class HighWarmLandRule : ABiomeLocationRule
{
    public override float Evaluate(BiomeConfig config, float elevationValue, float humidityValue, float erosionValue, float temperatureValue)
    {

        float eLow = SmoothStep01(1f - elevationValue);
        float eHigh = SmoothStep01(elevationValue);
        float tLow = SmoothStep01(1f - temperatureValue);
        float tHigh = SmoothStep01(temperatureValue);
        //return Mathf.Lerp(eHigh, eHigh * tHigh, 0.75f);

        float weight = elevationValue * temperatureValue;
        return weight;
    }
}
