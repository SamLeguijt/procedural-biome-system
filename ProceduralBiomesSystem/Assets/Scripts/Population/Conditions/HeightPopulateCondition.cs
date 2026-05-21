using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeightCondition", menuName = "ScriptableObjects/Population/Conditions/New HeightCondition")]
public class HeightPopulateCondition : APopulateCondition
{
    public int minHeight;
    public int maxHeight;

    public override bool Evaluate(PlacementContext context)
    {
        float height = context.AnalyseData.GetHeight(context.X, context.Y);

        return height >= minHeight && height <= maxHeight;
    }
}
