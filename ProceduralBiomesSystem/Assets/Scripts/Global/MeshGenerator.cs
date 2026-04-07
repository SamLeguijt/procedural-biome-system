using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Mesh;

public static class MeshGenerator 
{
    public static Mesh CreateMesh(Map<float> heightMap, float heightMultiplier = 1)
    {
        return CreateMesh(heightMap.Values, heightMultiplier);
    }

    public static Mesh CreateMesh(float[,] heightMap, float heightMultiplier = 1)
    {
        int meshResolutionMultiplier = 1;
        int width = heightMap.GetLength(0);
        int depth = heightMap.GetLength(1);

        int meshWidth = (width - 1) * meshResolutionMultiplier + 1;
        int meshDepth = (depth - 1) * meshResolutionMultiplier + 1;

        float topLeftX = (meshWidth - 1) / -2f;
        float topLeftZ = (meshDepth - 1) / 2f;

        Vector3[] vertices = new Vector3[meshWidth * meshDepth];
        List<int> triangles = new List<int>();

        int vertexIndex = 0;

        for (int z = 0; z < meshDepth; z++)
        {
            
            for (int x = 0; x < meshWidth; x++)
            {
                float percentX = x / (float)(meshWidth - 1);
                float percentZ = z / (float)(meshDepth - 1);
                
                int heightMapX = Mathf.FloorToInt(percentX * (width - 1));
                int heightMapZ = Mathf.FloorToInt(percentZ * (depth - 1));

                float height = heightMap[heightMapX, heightMapZ];

                vertices[vertexIndex] = new Vector3(topLeftX + x, height * heightMultiplier, topLeftZ - z);


                if (x < meshWidth - 1 && z < meshDepth - 1)
                {
                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + meshWidth + 1);
                    triangles.Add(vertexIndex + meshWidth);

                    triangles.Add(vertexIndex + meshWidth + 1);
                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }

    public static GameObject CreateGameObjectFromMesh(Mesh mesh, Material material, string objectName = "MeshObject")
    {
        GameObject go = new GameObject(objectName);
        MeshFilter meshFilter = go.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();

        meshFilter.mesh = mesh;
        meshRenderer.material = material;

        return go;
    }
}
