using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeightCondition", menuName = "ScriptableObjects/Population/Conditions/New AbsoluteHeightCondition")]
public class AbsoluteHeightPopulateCondition : APopulateCondition
{
    [SerializeField] private float minWorldHeight;
    [SerializeField] private float maxWorldHeight;

    public override bool Evaluate(PlacementContext context)
    {
        float height = context.AnalyseData.GetAbsoluteHeight(context.X, context.Y);

        return height >= minWorldHeight && height <= maxWorldHeight;
    }
}
