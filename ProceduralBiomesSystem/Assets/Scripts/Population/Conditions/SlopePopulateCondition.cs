using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlopeCondition_", menuName = "ScriptableObjects/Population/Conditions/New SlopeCondition")]
public class SlopePopulateCondition : APopulateCondition
{
    [SerializeField] private float minSlope = 0; 
    [SerializeField] private float maxSlope = 0;

    public override bool Evaluate(PlacementContext context)
    {
        float slopeValue = context.AnalyseData.SlopeMap[context.X, context.Y];

        return slopeValue >= minSlope && slopeValue <= maxSlope;
    }
}
