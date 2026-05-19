using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlacementRule_", menuName = "ScriptableObjects/PlacementRules")]
public class PlacementRule : ScriptableObject
{

    public GameObject objectToPlace;
    public List<APopulateCondition> conditions = new List<APopulateCondition>();

    // TODO: Refactor this class with updated system requirements. 
    // This should come in CandidateGenerator or similar 
    //public PlacementResult Evaluate(Vector3 position)
    //{
    //    if (objectToPlace == null || conditions == null || conditions.Count == 0)
    //        return new PlacementResult(null, position);

    //    foreach (APopulateCondition condition in conditions)
    //    {
    //        bool isMet = condition.Evaluate();

    //        if (!isMet)
    //            return new PlacementResult(null, position);
    //    }

    //    return new PlacementResult(objectToPlace, position);
    //}
}


