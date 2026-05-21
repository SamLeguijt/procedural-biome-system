using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultCandidatesResolver", menuName = "ScriptableObjects/Population/CandidatesResolver/new Default CandidatesResolver")]
public class DefaultPopulationCandidatesResolver : APopulationCandidatesResolver
{
    // This class defaults to returning all candidates.
    public override List<PopulationCandidate> Resolve(List<PopulationCandidate> candidates)
    {

        List<PopulationCandidate> result = new List<PopulationCandidate>();

        // already placed candidates (used for spatial checks)
        List<PopulationCandidate> accepted = new List<PopulationCandidate>();

        foreach (var candidate in candidates)
        {
            if (IsValid(candidate, accepted))
            {
                result.Add(candidate);
                accepted.Add(candidate);
            }
        }

        return result;
    }

    private bool IsValid(PopulationCandidate candidate, List<PopulationCandidate> accepted)
    {
        foreach (var other in accepted)
        {
            float minDistance = candidate.radius + other.radius;
            float sqrDistance = (candidate.worldPos - other.worldPos).sqrMagnitude;

            if (sqrDistance < minDistance * minDistance)
                return false;
        }

        return true;
    }
}
