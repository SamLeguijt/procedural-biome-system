using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CandidatesResolver_Random", menuName = "ScriptableObjects/Population/CandidatesResolver/New Random_CandidatesResolver")]
public class RandomPopulationCandidatesResolver : APopulationCandidatesResolver
{
    public override List<PopulateInstance> Resolve(List<PopulateCandidate> candidates)
    {
        return new List<PopulateInstance>();
    }
}
