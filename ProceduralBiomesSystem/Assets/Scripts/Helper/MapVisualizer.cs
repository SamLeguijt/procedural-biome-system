using Unity.VisualScripting;
using UnityEngine;


public enum MapDrawMode
{
    Elevation, 
    Erosion,
    Humidity,
    Temperature,
    Combined,
    Biomes, 
    Borders,
    BiomeDominance,
    ElevationTemperature
}

public class MapVisualizer : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private MapDrawMode mapDrawMode;

    WorldLayout recentLayoutDebug;
    WorldData recentWorldData;
        

    public void SetRecentLayout(WorldLayout layout)
    {
        if (layout != null)
            recentLayoutDebug = layout;


        Draw();
    }

    public void SetRecentWorld(WorldData data)
    {
        if (data.TerrainMap != null)
        {
            recentWorldData = data;
        }

        Draw();
    }

    private void Draw() 
    {
        if (recentLayoutDebug == null)
            return;

        switch (mapDrawMode)
        {
            case MapDrawMode.Elevation:
                DrawFloatMap(recentLayoutDebug.ElevationMap, Color.blue);
                break;

            case MapDrawMode.Erosion:
                DrawFloatMap(recentLayoutDebug.ErosionMap, Color.cyan);
                break;
            case MapDrawMode.Humidity:
                DrawFloatMap(recentLayoutDebug.HumidityMap, Color.green);
                break;
            case MapDrawMode.Temperature:
                DrawFloatMap(recentLayoutDebug.TemperatureMap, Color.red);
                break;
            case MapDrawMode.Biomes:
                if (recentWorldData != null)
                    DrawBiomeWeightColorsMap(recentWorldData.BiomeMap);
                break;
            case MapDrawMode.Combined:
                DrawCombinedMap(
                     (recentLayoutDebug.ElevationMap, Color.blue),
                     (recentLayoutDebug.HumidityMap, Color.green),
                     (recentLayoutDebug.ErosionMap, Color.cyan),
                     (recentLayoutDebug.TemperatureMap, Color.red)
                 );
                break;
            case MapDrawMode.Borders:
                if (recentWorldData != null)
                    DrawBiomeBlendMap(recentWorldData.BiomeMap);
                break;

            case MapDrawMode.BiomeDominance:
                if (recentWorldData != null)
                    DrawBiomeDominanceMap(recentWorldData.BiomeMap);
                break;

            case MapDrawMode.ElevationTemperature:
                DrawElevationTemperatureMap(
                    recentLayoutDebug.ElevationMap,
                    recentLayoutDebug.TemperatureMap
                );
                break;
        }
    }


    private void OnValidate()
    {
        if (Application.isPlaying)
            return;

        Draw();
    }

    public void DrawFloatMap(Map<float> map, Color mapColor)
    {
        int width = map.Width;
        int height = map.Height;

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Bilinear;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float value = map[x, y];
                Color color = Color.Lerp(Color.black, mapColor, value);

                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        targetRenderer.sharedMaterial.mainTexture = texture;
    }

    public void DrawFloatMapWithVertexColors(Map<float> map, Color[] vertexColors)
    {
        int width = map.Width;
        int height = map.Height;

        if (vertexColors.Length != width * height)
        {
            return;
        }

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = x + y * width;
                Color vertexColor = vertexColors[index];
                float value = map[x, y];
                texture.SetPixel(x, y, vertexColor );
            }
        }

        texture.Apply();
        targetRenderer.sharedMaterial.mainTexture = texture;
    }

    public void DrawBiomeWeightColorsMap(Map<BiomeWeights> map)
    {
        int width = map.Width;
        int height = map.Height;

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color color = map[x,y].ToColor();
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        targetRenderer.sharedMaterial.mainTexture = texture;
    }

    private void DrawBiomeBlendMap(Map<BiomeWeights> map)
    {
        int width = map.Width;
        int height = map.Height;

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                BiomeWeights weights = map[x, y];
                float border = GetBorderFactor(weights);

                Color color = Color.Lerp(Color.black, Color.white, border);

                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        targetRenderer.sharedMaterial.mainTexture = texture;
    }
    private void DrawBiomeDominanceMap(Map<BiomeWeights> map)
    {
        int width = map.Width;
        int height = map.Height;

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                BiomeWeights weights = map[x, y];

                float maxWeight = 0f;

                foreach (var kvp in weights.ConfigWeights)
                    if (kvp.Value > maxWeight)
                        maxWeight = kvp.Value;

                // white = strong biome, black = weak
                Color color = Color.Lerp(Color.black, Color.white, maxWeight);

                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        targetRenderer.sharedMaterial.mainTexture = texture;
    }
    private float GetBorderFactor(BiomeWeights weights)
    {
        float maxWeight = 0f;
        float secondMax = 0f;

        foreach (var kvp in weights.ConfigWeights)
        {
            float w = kvp.Value;

            if (w > maxWeight)
            {
                secondMax = maxWeight;
                maxWeight = w;
            }
            else if (w > secondMax)
            {
                secondMax = w;
            }
        }

        float diff = maxWeight - secondMax;

        return 1f - Mathf.Clamp01(diff * 5f);
    }
    public void DrawCombinedMap((Map<float>, Color) mapA, (Map<float>, Color) mapB, (Map<float>, Color) mapC, (Map<float>, Color) mapD)
    {
        int width = mapA.Item1.Width;
        int height = mapA.Item1.Height;

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float a = mapA.Item1[x, y];
                float b = mapB.Item1[x, y];
                float c = mapC.Item1[x, y];
                float d = mapD.Item1[x, y];

                Color colorA = mapA.Item2 * a;
                Color colorB = mapB.Item2 * b;
                Color colorC = mapC.Item2 * c;
                Color colorD = mapD.Item2 * d;

                Color pixelColor = colorA + colorB + colorC + colorD;

                texture.SetPixel(x, y, pixelColor);
            }
        }

        texture.Apply();
        targetRenderer.sharedMaterial.mainTexture = texture;
    }

    public void DrawSingleBiomeWeight(Map<BiomeWeights> map, BiomeConfig targetBiome)
    {
        int width = map.Width;
        int height = map.Height;

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float weight = map[x, y].GetWeight(targetBiome);

                Color color = Color.Lerp(Color.black, Color.white, weight);

                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        targetRenderer.sharedMaterial.mainTexture = texture;
    }

    public void DrawBorderFactorMap(Map<BiomeWeights> map)
    {
        int width = map.Width;
        int height = map.Height;

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float border = GetBorderFactor(map[x, y]);

                // black = no blend, white = strong blend
                Color color = Color.Lerp(Color.black, Color.white, border);

                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        targetRenderer.sharedMaterial.mainTexture = texture;
    }

    private void DrawElevationTemperatureMap(Map<float> elevation, Map<float> temperature)
    {
        int width = elevation.Width;
        int height = elevation.Height;

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float e = elevation[x, y];      
                float t = temperature[x, y];  

                Color baseColor = Color.Lerp(Color.black, Color.white, e);
                Color tempColor = Color.Lerp(Color.blue, Color.red, t);
                Color finalColor = baseColor * tempColor;

                texture.SetPixel(x, y, finalColor);
            }
        }

        texture.Apply();
        targetRenderer.sharedMaterial.mainTexture = texture;
    }
}