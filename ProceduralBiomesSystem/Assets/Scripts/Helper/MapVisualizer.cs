using Unity.VisualScripting;
using UnityEngine;


public enum MapDrawMode
{
    Elevation, 
    Erosion,
    Humidity,
    Combined,
    Biomes, 
    DesertSample,
    MountainSample,
    VolcanicSample,
    PlainsSample
}

public class MapVisualizer : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;

    [SerializeField] private MapDrawMode mapDrawMode;

    WorldLayout recentLayoutDebug;

    [SerializeField] AbstractMeshTerrainGenerator MountainGenerator;

    /// Should reference the generators, then subscribe to generators on value changed event, 
    /// Then have enum option to draw those maps 

    void GetDependencies()
    {
        MountainGenerator.OnValueChanged += Draw;
    }

    public void SetRecentLayout(WorldLayout layout)
    {
        if (layout != null)
            recentLayoutDebug = layout;

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
                DrawFloatMap(recentLayoutDebug.ErosionMap, Color.green);
                break;

            case MapDrawMode.Humidity:
                DrawFloatMap(recentLayoutDebug.HumidityMap, Color.red);
                break;
            case MapDrawMode.Biomes:
                DrawBiomeMap(recentLayoutDebug.BiomeMap);
                break;
            case MapDrawMode.Combined:
                DrawCombinedMap(
                     (recentLayoutDebug.ElevationMap, Color.blue),
                     (recentLayoutDebug.HumidityMap, Color.red),
                     (recentLayoutDebug.ErosionMap, Color.green)
                 );
                break;
            case MapDrawMode.MountainSample:
                int width = recentLayoutDebug.ElevationMap.Width;
                int height = recentLayoutDebug.ElevationMap.Height;
                DrawFloatMap(MountainGenerator.GenerateHeightMap(width, height), Color.green);
                break;
        }
    }


    private void OnValidate()
    {
        if (Application.isPlaying)
            return;

        GetDependencies();
        Draw();
    }

    public void DrawFloatMap(Map<float> map, Color mapColor)
    {
        int width = map.Width;
        int height = map.Height;

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float value = map[x, y];
                Color color = Color.Lerp(Color.white, mapColor, value);

                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        targetRenderer.sharedMaterial.mainTexture = texture;
    }

    public void DrawBiomeMap(Map<BiomeWeights> map)
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

    public void DrawCombinedMap((Map<float>, Color) mapA, (Map<float>, Color) mapB, (Map<float>, Color) mapC)
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

                Color colorA = mapA.Item2 * a;
                Color colorB = mapB.Item2 * b;
                Color colorC = mapC.Item2 * c;

                Color pixelColor = colorA + colorB + colorC;

                texture.SetPixel(x, y, pixelColor);
            }
        }

        texture.Apply();
        targetRenderer.sharedMaterial.mainTexture = texture;
    }
}