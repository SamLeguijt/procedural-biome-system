using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ALayoutGenerator : ScriptableObject, IWorldLayoutGenerator
{
    public Action<WorldLayout> OnLayoutChanged;
    public abstract WorldLayout GenerateWorldLayout(WorldSettings settings);
}
