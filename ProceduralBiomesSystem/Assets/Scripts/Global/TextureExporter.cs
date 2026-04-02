using System.IO;
using UnityEngine;

public static class TextureExporter
{
    public static void SaveFloatMapAsTexture(Map<float> map, string fileName)
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
                Color color = Color.Lerp(Color.black, Color.white, value);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();

        string path = Application.dataPath + "/GeneratedTextures/" + fileName + ".png";

        // Ensure folder exists
        Directory.CreateDirectory(Path.GetDirectoryName(path));

        File.WriteAllBytes(path, bytes);

        Debug.Log("Saved texture to: " + path);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}