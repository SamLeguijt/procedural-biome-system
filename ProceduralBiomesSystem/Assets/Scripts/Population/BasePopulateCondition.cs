using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasePopulateCondition
{
    /// <summary>
    /// TEMP IMPLEMENTATION
    /// 
    /// Returns if value < threshold
    /// </summary>
    /// <param name="value"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    public bool Evaluate(float value, float threshold)
    {
        return value < threshold;
    }
}
