using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlacementRule_", menuName = "ScriptableObjects/PlacementRules")]
public class PlacementRule : ScriptableObject
{
    [field: SerializeField] public List<GameObject> PrefabVariations = new List<GameObject>();
    [field: SerializeField] public float OccupationRadius { get; private set; } = 0f;
    [field: SerializeField] public List<APopulateCondition> Conditions { get; private set; } = new List<APopulateCondition>();


    public bool Evaluate(PlacementContext context)
    {
        if (PrefabVariations.Count == 0)
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

    public GameObject GetRandomVariation()
    {
        if (PrefabVariations.Count == 0)
            throw new System.Exception("No possible prefab variation to randomly select!");

        int randomIndex = Random.Range(0, PrefabVariations.Count);
        return PrefabVariations[randomIndex];
    }
}


