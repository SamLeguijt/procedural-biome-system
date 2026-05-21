using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CandidatesResolver_Random", menuName = "ScriptableObjects/Population/CandidatesResolver/New Random_CandidatesResolver")]
public class RandomBasedCandidatesResolver : APopulationCandidatesResolver
{
    [SerializeField] private int maxAllowedInstances; 

    public override List<PopulationCandidate> Resolve(List<PopulationCandidate> candidates)
    {
        List<PopulationCandidate> result = new List<PopulationCandidate>();
        Debug.Log("Received: " + candidates.Count);

        int count = Mathf.Min(maxAllowedInstances, candidates.Count);

        List<PopulationCandidate> available = new List<PopulationCandidate>(candidates);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, available.Count);

            result.Add(available[randomIndex]);

            available.RemoveAt(randomIndex);
        }

        Debug.Log("Resolved: " + result.Count);
        return result;

    }
}
