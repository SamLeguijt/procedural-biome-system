using System;
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

    public float Highest => Mathf.Max(mountainsWeight, volcanicWeight, desertWeight, plainsWeight); 

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

    public (EBiome, float) GetHighestWeight()
    {
        EBiome highestBiome = EBiome.Plains;
        float highestWeight = PlainsWeight;

        if (MountainsWeight > highestWeight)
        {
            highestBiome = EBiome.Mountains;
            highestWeight = MountainsWeight;
        }

        if (VolcanicWeight > highestWeight)
        {
            highestBiome = EBiome.Volcanic;
            highestWeight = VolcanicWeight;
        }

        if (DesertWeight > highestWeight)
        {
            highestBiome = EBiome.Desert;
            highestWeight = DesertWeight;
        }

        return (highestBiome, highestWeight);
    }

    public (EBiome biome, float weight)[] GetSortedWeights()
    {
        var result = new (EBiome, float)[4]
        {
        (EBiome.Mountains, MountainsWeight),
        (EBiome.Volcanic, VolcanicWeight),
        (EBiome.Desert, DesertWeight),
        (EBiome.Plains, PlainsWeight)
        };

        for (int i = 0; i < result.Length - 1; i++)
        {
            for (int j = i + 1; j < result.Length; j++)
            {
                if (result[j].Item2 > result[i].Item2)
                {
                    var temp = result[i];
                    result[i] = result[j];
                    result[j] = temp;
                }
            }
        }

        return result;
    }

    public float GetWeight(EBiome biomeType)
    {
        switch (biomeType)
        {
            case EBiome.Mountains:
                return mountainsWeight;
            case EBiome.Volcanic:
                return volcanicWeight;
            case EBiome.Desert:
                return desertWeight;
            case EBiome.Plains:
                return plainsWeight;
        }

        return 0f;
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

        float max = Mathf.Max(DesertWeight, MountainsWeight, VolcanicWeight, PlainsWeight);

        if (max == DesertWeight)
            return Color.yellow;

        if (max == MountainsWeight)
            return Color.green;

        if (max == VolcanicWeight)
            return Color.red;

        return Color.cyan;
    }
}
