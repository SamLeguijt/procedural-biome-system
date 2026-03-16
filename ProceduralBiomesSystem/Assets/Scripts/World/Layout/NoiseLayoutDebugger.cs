using UnityEngine;

public class NoiseLayoutDebugger : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] int width = 200;
    [SerializeField] int height = 200;

    [Header("Noise Settings")]
    [SerializeField] int seed = 0;
    [SerializeField] float scale = 50;
    [SerializeField] int octaves = 4;
    [SerializeField] float persistance = 0.5f;
    [SerializeField] float lacunarity = 2f;
    [SerializeField] Vector2 offset;
    [SerializeField] public int heightMultiplier ;

    public float[] regionThresholds = new float[2];

    [Header("Debug")]
    [SerializeField] bool autoUpdate = true;

    Texture2D debugTexture;
    float[,] noiseMap;

    void OnValidate()
    {
        if (!autoUpdate) return;

        Generate();
    }

    void Generate()
    {
        noiseMap = Utils.GenerateNoiseMap(
            width,
            height,
            seed,
            scale,
            octaves,
            persistance,
            lacunarity,
            offset
        );

        debugTexture = GenerateTexture(noiseMap);
    }

    Texture2D GenerateTexture(float[,] map)
    {
        int w = map.GetLength(0);
        int h = map.GetLength(1);

        Texture2D tex = new Texture2D(w, h);
        tex.filterMode = FilterMode.Point;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {

                float v = map[x, y];
                tex.SetPixel(x, y, Color.Lerp(Color.black, Color.white, v));
            }
        }

        tex.Apply();
        return tex;
    }

    void OnDrawGizmos()
    {
        if (noiseMap == null)
            return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float value = noiseMap[x, y];
                Color color;

                if (value < regionThresholds[0])
                {
                    color = Color.black;
                }
                else if (value < regionThresholds[1])
                {
                    color = Color.gray;
                }
                else 
                {
                    color = Color.white;
                }

                Gizmos.color = color;

                Vector3 pos = new Vector3(x, 1 * value * heightMultiplier  , y);

                Gizmos.DrawCube(pos, Vector3.one * 0.9f);
            }
        }
    }
}