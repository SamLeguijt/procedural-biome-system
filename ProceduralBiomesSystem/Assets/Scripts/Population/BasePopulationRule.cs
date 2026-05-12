using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlacementRule_", menuName = "ScriptableObjects/PlacementRules")]
public class BasePopulationRule : ScriptableObject
{
    // BiomeConfig holds PlacementRules
    // PlacementRules wraps a prefab with a set of conditions
    // If all conditions are met, the prefab can be placed. 
    // Rule only exists to see if x object can be placed at x position, depending on condition. 
    // Should return the object to place, as well as the position. 


    // Concrete example rule: 
    // A tree may be placed if the slope of that location is < x

    public GameObject objectToPlace;
    public List<BasePopulateCondition> conditions = new List<BasePopulateCondition>();

    public PlacementResult Evaluate(Vector3 position)
    {
        if (objectToPlace || conditions == null || conditions.Count == 0)
            return new PlacementResult(null, position);

        foreach (BasePopulateCondition condition in conditions)
        {
            bool isMet = condition.Evaluate(5, 10);

            if (!isMet)
                return new PlacementResult(null, position);
        }

        return new PlacementResult(objectToPlace, position);
    }
}

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

public struct PlacementResult
{
    public bool IsValid => Prefab != null;

    public GameObject Prefab { get; private set; }
    public Vector3 Position { get; private set; }

    public PlacementResult(GameObject prefab, Vector3 position)
    {
        Prefab = prefab;
        Position = position;
    }
}