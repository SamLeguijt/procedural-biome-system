using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABiomeLocationRule : ScriptableObject
{
    public abstract float Evaluate(BiomeConfig config, float elevationValue, float humidityValue, float erosionValue, float temperatureValue);
    
    protected float SmoothStep01(float value)
    {
        return value * value * (3f - 2f * value); 
    }
}
