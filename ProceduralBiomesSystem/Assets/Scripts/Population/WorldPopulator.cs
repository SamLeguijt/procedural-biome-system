using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldPopulator_", menuName = "ScriptableObjects/Population/New WorldPopulator")]
public class WorldPopulator : ScriptableObject
{
    [field: SerializeField] public APopulationCandidatesResolver populationCandidatesResolver {  get; private set; }

    public List<PopulateInstance> PopulateWorld(WorldAnalysisData analysisData)
    {
        List<PopulateCandidate> candidates = new List<PopulateCandidate>();
        List<PopulateInstance> resolvedCandidates = populationCandidatesResolver.Resolve(candidates);
        
        return resolvedCandidates;
    }

    public List<PopulateCandidate> GenerateCandidates(WorldAnalysisData analysisData)
    {
        // TODO: implementation.
        return new List<PopulateCandidate>();
    }
}
