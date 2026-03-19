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

    public void DrawFloatMap(Map<float> map)
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
                float value = map[x, y]; // assumed 0–1
                Color color = Color.Lerp(Color.black, Color.white, value);

                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        targetRenderer.sharedMaterial.mainTexture = texture;
    }

    public void DrawBiomeMap(Map<EBiome> map)
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

                switch (map[x, y])
                {
                    case EBiome.Desert:
                        color = Color.yellow;
                        break;
                    case EBiome.Mountains:
                        color = Color.gray;
                        break;
                    case EBiome.Volcanic:
                        color = Color.red;
                        break;
                    case EBiome.Plains:
                        color = Color.green;
                        break;
                }

                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        targetRenderer.sharedMaterial.mainTexture = texture;
    }
}