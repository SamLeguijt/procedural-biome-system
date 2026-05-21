using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlacementRule_", menuName = "ScriptableObjects/PlacementRules")]
public class PlacementRule : ScriptableObject
{
    public GameObject Prefab { get; private set; } = null; 
    public float OccupationRadius { get; private set; } = 0f;
    public List<APopulateCondition> Conditions { get; private set; } = new List<APopulateCondition>();


    public bool Evaluate(PlacementContext context)
    {
        if (Prefab == null || Conditions == null || Conditions.Count == 0)
            return false;

        foreach (APopulateCondition condition in Conditions)
        {
            bool isMet = condition.Evaluate(context);

            if (!isMet)
                return false;
        }

        return true; 
    }
}


