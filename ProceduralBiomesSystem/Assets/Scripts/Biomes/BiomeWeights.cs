using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BiomeWeights
{
    public float MountainsWeight => mountainsWeight;
    public float VolcanicWeight => volcanicWeight;
    public float DesertWeight => desertWeight;
    public float PlainsWeight => plainsWeight;

    private float mountainsWeight;
    private float volcanicWeight;
    private float desertWeight;
    private float plainsWeight;

    public BiomeWeights(float _mountainsWeight, float _volcanicWeight, float _desertWeight, float _plainsWeight)
    {
        mountainsWeight = _mountainsWeight;
        volcanicWeight = _volcanicWeight;
        desertWeight = _desertWeight;
        plainsWeight = _plainsWeight;

        Normalise(); 
    }

    private void Normalise()
    {
        float sum = mountainsWeight + volcanicWeight + desertWeight + plainsWeight;

        if (sum > 0f)
        {
            mountainsWeight /= sum;
            volcanicWeight /= sum;
            desertWeight /= sum;
            plainsWeight /= sum;
        }
    }

    /// <summary>
    /// Returns a color representing the stored weights: 
    /// R -> Desert
    /// G -> Mountains
    /// B -> Volcanic
    /// A -> Plains
    /// </summary>
    /// <returns></returns>
    public Color ToColor()
    {
        return new Color(DesertWeight, MountainsWeight, VolcanicWeight, PlainsWeight); 
    }
}
