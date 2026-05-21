using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class APopulateCondition : ScriptableObject 
{
    public abstract bool Evaluate(PlacementContext context);
}
