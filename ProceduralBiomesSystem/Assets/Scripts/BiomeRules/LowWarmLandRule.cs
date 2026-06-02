using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LowHeightHighTempRule", menuName = "ScriptableObjects/Biomes/Rules/new low-height, high-temp rule")]
public class LowWarmLandRule : ABiomeLocationRule
{
    public override float Evaluate(BiomeConfig config, float elevationValue, float humidityValue, float erosionValue, float temperatureValue)
    {
        float eLow = SmoothStep01(1f - elevationValue);
        float eHigh = SmoothStep01(elevationValue);
        float tLow = SmoothStep01(1f - temperatureValue);
        float tHigh = SmoothStep01(temperatureValue);
        //return Mathf.Lerp(eLow, eLow * tHigh, 0.75f);

        float weight = (1f - elevationValue) * temperatureValue;
        return weight;
    }
}
