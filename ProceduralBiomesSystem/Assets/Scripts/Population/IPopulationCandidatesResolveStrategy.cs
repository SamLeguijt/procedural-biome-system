using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPopulationCandidatesResolveStrategy 
{
    List<PopulateInstance> Resolve(List<PopulateCandidate> candidates);
}
