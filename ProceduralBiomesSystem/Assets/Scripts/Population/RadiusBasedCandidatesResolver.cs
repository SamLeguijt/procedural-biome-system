using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultCandidatesResolver", menuName = "ScriptableObjects/Population/CandidatesResolver/new Default CandidatesResolver")]
public class RadiusBasedCandidatesResolver : APopulationCandidatesResolver
{
    public override List<PopulationCandidate> Resolve(List<PopulationCandidate> candidates)
    {
        List<PopulationCandidate> result = new List<PopulationCandidate>();
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
            float minDistance = candidate.OccupationRadius + other.OccupationRadius;
            float sqrDistance = (candidate.WorldPosition - other.WorldPosition).sqrMagnitude;

            if (sqrDistance < minDistance * minDistance)
                return false;
        }

        return true;
    }
}
