using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractLayoutGenerator : ScriptableObject, IWorldLayoutGenerator
{
    public Action<WorldLayout> OnLayoutChanged;
    public abstract WorldLayout GenerateWorldLayout(WorldSettings settings);
}
