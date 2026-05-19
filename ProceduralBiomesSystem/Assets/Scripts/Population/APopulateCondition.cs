using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class APopulateCondition
{
    public abstract bool Evaluate(PlacementContext context);
}
