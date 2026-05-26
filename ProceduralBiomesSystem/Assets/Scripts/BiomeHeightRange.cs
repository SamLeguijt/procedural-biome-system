using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class BiomeHeightRange 
{
    public float Min => min;
    public float Max => max;


    private float min;
    private float max;

    public BiomeHeightRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    private float GetValueInRange(float value)
    {
        return Mathf.InverseLerp(Min, Max, value);
    }

    public void Encapsulate(float value)
    {
        if (value < Min)
            min = value;

        if (value > Max) 
            max = value;
    }
}
