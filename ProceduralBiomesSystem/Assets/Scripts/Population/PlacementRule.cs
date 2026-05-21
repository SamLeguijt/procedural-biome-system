using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlacementRule_", menuName = "ScriptableObjects/PlacementRules")]
public class PlacementRule : ScriptableObject
{
    [field: SerializeField] public GameObject Prefab { get; private set; } = null;
    [field: SerializeField] public float OccupationRadius { get; private set; } = 0f;
    [field: SerializeField] public List<APopulateCondition> Conditions { get; private set; } = new List<APopulateCondition>();


    public bool Evaluate(PlacementContext context)
    {
        if (Prefab == null)
            return false;

        // TO BE DETERMINED: 
        // Should pass if conditions is empty/null?
        if (Conditions == null || Conditions.Count == 0)
            return true;

        foreach (APopulateCondition condition in Conditions)
        {
            bool isMet = condition.Evaluate(context);

            if (!isMet)
                return false;
        }


        return true; 
    }
}


