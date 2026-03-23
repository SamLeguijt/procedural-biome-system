using Unity.VisualScripting;
using UnityEngine;


public enum MapDrawMode
{
    None,
    Elevation, 
    Erosion,
    Humidity,
    Biomes,
    All
}

public class MapVisualizer : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;

    public void DrawFloatMap(Map<float> map, Color mapColor)
    {
        if (map == null)
        {
            Debug.Log("null");
        }
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
                Color color = Color.black;

                //switch (map[x, y])
                //{
                //    case EBiome.Desert:
                //        color = Color.yellow;
                //        break;
                //    case EBiome.Mountains:
                //        color = Color.gray;
                //        break;
                //    case EBiome.Volcanic:
                //        color = Color.red;
                //        break;
                //    case EBiome.Plains:
                //        color = Color.green;
                //        break;
                //}

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