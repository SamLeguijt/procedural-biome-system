using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class WorldGenDebugger : MonoBehaviour
{
    public BiomeWorldGenerator generator;
    public Dictionary<EBiome, Map<float>> debugMaps => generator.recentBiomeMaps;

    private void OnDrawGizmosSelected()
    {
        if (generator == null || debugMaps == null)
            return;

        foreach (var kvp in debugMaps)
        {
            var map = kvp.Value;

            int width = map.Width;
            int height = map.Height;

            float topLeftX = (width - 1) / -2f;
            float topLeftZ = (height - 1) / 2f;

            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    float value = map[x, z];


                    if (Mathf.Abs(value) < 0.01f)
                    {
                        Vector3 pos = new Vector3(
                            topLeftX + x,
                            value * generator.heightMultiplier,
                            topLeftZ - z
                        );


                        Color color = Color.white;
                        switch (kvp.Key)
                        {
                            case EBiome.Mountains: color = Color.green; break;
                            case EBiome.Volcanic: color = Color.blue; break;
                            case EBiome.Desert: color = Color.yellow; break;
                            case EBiome.Plains: color = Color.cyan; break;
                        }

                        Gizmos.color = color;
                        Gizmos.DrawSphere(pos, 0.2f);
                    }
                }
            }
        }
    }
}