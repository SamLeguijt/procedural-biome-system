using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HighHeightLowTempRule", menuName = "ScriptableObjects/Biomes/Rules/new high-height, low-temp rule")]
public class HighColdLandRule : ABiomeLocationRule
{
    public override float Evaluate(BiomeConfig config, float elevationValue, float humidityValue, float erosionValue, float temperatureValue)
    {

        float eLow = SmoothStep01(1f - elevationValue);
        float eHigh = SmoothStep01(elevationValue);
        float tLow = SmoothStep01(1f - temperatureValue);
        float tHigh = SmoothStep01(temperatureValue);
        //return Mathf.Lerp(eHigh, eHigh * tLow, 0.75f);


        float weight = elevationValue * (1f - temperatureValue);
        return weight;
    }
}
