using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlacementRule_", menuName = "ScriptableObjects/PlacementRules")]
public class PlacementRule : ScriptableObject
{
    public GameObject objectToPlace;
    public float radius; 
    public List<APopulateCondition> conditions = new List<APopulateCondition>();

    public bool Evaluate(PlacementContext context)
    {
        if (objectToPlace == null || conditions == null || conditions.Count == 0)
            return false;

        foreach (APopulateCondition condition in conditions)
        {
            bool isMet = condition.Evaluate(context);

            if (!isMet)
                return false;
        }

        return true; 
    }
}


