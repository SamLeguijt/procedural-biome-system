using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalisedHeightCondition", menuName = "ScriptableObjects/Population/Conditions/New NormalisedHeightCondition")]
public class HeightNormalisedPopulateCondition : APopulateCondition
{
    [SerializeField, Range(0, 1)] private float min = 0;
    [SerializeField, Range(0, 1)] private float max = 0;

    public override bool Evaluate(PlacementContext context)
    {
        BiomeConfig primaryBiome = context.AnalyseData.GetPrimaryBiome(context.X, context.Y);
        float height = context.AnalyseData.GetNormalisedHeight(context.X, context.Y, primaryBiome);

        return height >= min && height <= max;
    }
}
