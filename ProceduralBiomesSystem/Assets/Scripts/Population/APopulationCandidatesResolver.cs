using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class APopulationCandidatesResolver : ScriptableObject, IPopulationCandidatesResolveStrategy
{
    public abstract List<PopulateInstance> Resolve(List<PopulateCandidate> candidates);
}
