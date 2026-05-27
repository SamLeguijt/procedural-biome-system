using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ValueRange 
{
    [SerializeField] private float min;
    [SerializeField] private float max;

    public float Min => min;
    public float Max => max;

    public bool FallsInRange(float value)
    {
        return value >= min && value <= max;
    }
}

[System.Serializable]
public struct ValueRangeNormalised 
{
    [SerializeField, Range(0,1)] private float min;
    [SerializeField, Range(0,1)] private float max;

    public float Min => min;
    public float Max => max;
    public bool FallsInRange(float value)
    {
        return value >= min && value <= max;
    }
}
