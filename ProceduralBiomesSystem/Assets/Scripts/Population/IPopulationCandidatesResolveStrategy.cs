using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPopulationCandidatesResolveStrategy 
{
    List<PopulationCandidate> Resolve(List<PopulationCandidate> candidates);
}
