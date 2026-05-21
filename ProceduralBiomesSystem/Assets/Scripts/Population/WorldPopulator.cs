using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldPopulator_", menuName = "ScriptableObjects/Population/New WorldPopulator")]
public class WorldPopulator : ScriptableObject
{
    [field: SerializeField] public APopulationCandidatesResolver populationCandidatesResolver {  get; private set; }

    public List<PopulationCandidate> PopulateWorld(WorldAnalysisData analysisData)
    {
        List<PopulationCandidate> candidates = GenerateCandidates(analysisData);
        List<PopulationCandidate> resolvedCandidates = populationCandidatesResolver.Resolve(candidates);
        
        return resolvedCandidates;
    }

    public List<PopulationCandidate> GenerateCandidates(WorldAnalysisData analysisData)
    {
        List<PopulationCandidate> result = new List<PopulationCandidate>();
    
        var heightMap = analysisData.TerrainData.HeightMap;
        var slopeMap = analysisData.TerrainData.SlopeMap;
        var primaryBiomeMap = analysisData.TerrainData.PrimaryBiomeMap;


        for (int y = 0; y < primaryBiomeMap.Height; y++)
        {
            for (int x = 0; x < primaryBiomeMap.Width; x++)
            {
                BiomeConfig config = primaryBiomeMap[x, y];
                Vector3 worldPosition = TerrainSpaceUtils.GridToTerrainWorld(x, y, primaryBiomeMap.Width, primaryBiomeMap.Height, heightMap[x, y]);
                
                PlacementContext context = new PlacementContext
                    (
                        x, y, worldPosition, analysisData.TerrainData
                    );

                if (config == null)
                    continue;

                foreach (PlacementRule rule in config.PopulationRules)
                {
                    bool ruleMet = rule.Evaluate(context);

                    if (ruleMet)

                    {
                        PopulationCandidate candidate = new PopulationCandidate
                            (
                                rule.objectToPlace,
                                worldPosition,
                                Quaternion.identity,
                                rule.radius
                            );

                        result.Add(candidate);
                    }
                }
            }
        }


        return result;
    }
}
