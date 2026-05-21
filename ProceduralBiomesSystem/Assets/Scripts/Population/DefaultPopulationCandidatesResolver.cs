using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultCandidatesResolver", menuName = "ScriptableObjects/Population/CandidatesResolver/new Default CandidatesResolver")]
public class DefaultPopulationCandidatesResolver : APopulationCandidatesResolver
{
    // This class defaults to returning all candidates.
    public override List<PopulationCandidate> Resolve(List<PopulationCandidate> candidates)
    {

        return candidates;
    }
}
