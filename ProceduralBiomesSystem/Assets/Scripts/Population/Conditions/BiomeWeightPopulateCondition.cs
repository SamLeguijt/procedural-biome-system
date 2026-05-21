using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeWeightCondition_", menuName = "ScriptableObjects/Population/Conditions/New BiomeWeightCondition")]
public class BiomeWeightPopulateCondition : APopulateCondition
{
    [SerializeField] private float minWeight = 0;
    [SerializeField] private float maxWeight = 0;

    public override bool Evaluate(PlacementContext context)
    {
        BiomeWeights biomeWeights = context.AnalyseData.GetBiomeWeights(context.X, context.Y);
        var highest = biomeWeights.GetHighest();
        float weightValue = highest.Item2;

        return (weightValue > minWeight && weightValue <= maxWeight);
    }
}
