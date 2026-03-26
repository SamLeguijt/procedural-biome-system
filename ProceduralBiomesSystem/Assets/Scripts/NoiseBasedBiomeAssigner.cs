using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoiseBased_BiomeAssigner", menuName = "ScriptableObjects/Biomes/new NoiseBasedBiomeAssigner")]
public class NoiseBasedBiomeAssigner : BaseBiomeAssigner
{
    [SerializeField] private float sharpness = 1;

    public override Map<BiomeWeights> GenerateBiomeMap(WorldLayout layout)
    {
        Map<float> elevationMap = layout.ElevationMap;
        Map<float> erosionMap = layout.ErosionMap;
        Map<float> humidityMap = layout.HumidityMap;

        int width = elevationMap.Width;
        int height = elevationMap.Height;

        Map<BiomeWeights> biomeMap = new Map<BiomeWeights>(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float elevationValue = elevationMap[x, y];
                float erosionValue = erosionMap[x, y];
                float humidityValue = humidityMap[x, y];

                /// TODO: Use 'BiomeSet' instead somehow. 
                float desert = (1f - humidityValue) * (1f - elevationValue);
                float mountains = elevationValue * (1f - erosionValue);
                float plains = (1f - elevationValue) * humidityValue;
                float volcanic = elevationValue * erosionValue;

                mountains = Mathf.Pow(mountains, sharpness);
                desert = Mathf.Pow(desert, sharpness);
                plains = Mathf.Pow(plains, sharpness);
                volcanic = Mathf.Pow(volcanic, sharpness);

                biomeMap[x, y] = new BiomeWeights(mountains, volcanic, desert, plains);
            }
        }

        return biomeMap;
    }
}
