using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBiomeRule : ScriptableObject
{
    public abstract float Evaluate(BiomeConfig config, float elevationValue, float humidityValue, float erosionValue, float temperatureValue);
}
