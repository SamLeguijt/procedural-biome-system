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

        MeshData meshData = MeshGen.GenerateMesh(heightMap.Values);
        Mesh mesh = meshData.CreateMesh();
        return mesh;
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
        Vector3[] normals = new Vector3[vertices.Length];
        Vector2[] uvs = new Vector2[meshWidth * meshDepth];

    int vertexIndex = 0;

        for (int z = 0; z < meshDepth; z++)
        {
            
            for (int x = 0; x < meshWidth; x++)
            {
                float percentX = x / (float)(meshWidth - 1);
                float percentZ = z / (float)(meshDepth - 1);
                
                float heightMapX = percentX * (width - 1);
                float heightMapZ = percentZ * (depth - 1);

                int x0 = Mathf.FloorToInt(heightMapX);
                int x1 = Mathf.Clamp(x0 + 1, 0, width - 1);

                int z0 = Mathf.FloorToInt(heightMapZ);
                int z1 = Mathf.Clamp(z0 + 1, 0, depth - 1);

                float tx = heightMapX - x0;
                float tz = heightMapZ - z0;

                float h00 = heightMap[x0, z0];
                float h10 = heightMap[x1, z0];
                float h01 = heightMap[x0, z1];
                float h11 = heightMap[x1, z1];

                float height = Mathf.Lerp(
                    Mathf.Lerp(h00, h10, tx),
                    Mathf.Lerp(h01, h11, tx),
                    tz
                );

                vertices[vertexIndex] = new Vector3(topLeftX + x, height * heightMultiplier, topLeftZ - z);
                uvs[vertexIndex] = new Vector2(x / (float)width, z / (float)height);



                //float percentX = x / (float)(meshWidth - 1);
                //int heightMapX = Mathf.FloorToInt(percentX * (width - 1));

                //float height = heightMap[heightMapX, heightMapZ] * heightMultiplier;
                //vertices[vertexIndex] = new Vector3(topLeftX + x, height, topLeftZ - z);

                if (x < meshWidth - 1 && z < meshDepth - 1)
                {
                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + meshWidth + 1);
                    triangles.Add(vertexIndex + meshWidth);

                    triangles.Add(vertexIndex + meshWidth + 1);
                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + 1);



                    //triangles.Add(vertexIndex);
                    //triangles.Add(vertexIndex + meshWidth + 1);
                    //triangles.Add(vertexIndex + meshWidth);

                    //triangles.Add(vertexIndex + meshWidth + 1);
                    //triangles.Add(vertexIndex);
                    //triangles.Add(vertexIndex + 1);
                }

                int i = z * meshWidth + x;

                int xL = Mathf.Max(x - 1, 0);
                int xR = Mathf.Min(x + 1, meshWidth - 1);
                int zD = Mathf.Max(z - 1, 0);
                int zU = Mathf.Min(z + 1, meshDepth - 1);

                float hL = vertices[z * meshWidth + xL].y;
                float hR = vertices[z * meshWidth + xR].y;
                float hD = vertices[zD * meshWidth + x].y;
                float hU = vertices[zU * meshWidth + x].y;

                Vector3 normal = new Vector3(hL - hR, 2f, hD - hU).normalized;
                normals[i] = normal;


                vertexIndex++;
            }
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs;
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


public static class MeshGen

{

    public static MeshData GenerateMesh(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = width / 2f;
        float topLeftZ = height / 2f;
        float heightMultiplier = 50;

        MeshData meshData = new MeshData(width, height);

        int vertI = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                
                float percentX = x / (float)(width - 1);
                float percentZ = y / (float)(height - 1);

                float heightMapX = percentX * (width - 1);
                float heightMapZ = percentZ * (height - 1);

                int x0 = Mathf.FloorToInt(heightMapX);
                int x1 = Mathf.Clamp(x0 + 1, 0, width - 1);

                int z0 = Mathf.FloorToInt(heightMapZ);
                int z1 = Mathf.Clamp(z0 + 1, 0, height - 1);

                float tx = heightMapX - x0;
                float tz = heightMapZ - z0;

                float h00 = heightMap[x0, z0];
                float h10 = heightMap[x1, z0];
                float h01 = heightMap[x0, z1];
                float h11 = heightMap[x1, z1];

                float vertexHeight = Mathf.Lerp(
                    Mathf.Lerp(h00, h10, tx),
                    Mathf.Lerp(h01, h11, tx),
                    tz
                );

                meshData.vertices[vertI] = new Vector3(topLeftX + x, vertexHeight * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertI] = new Vector2(x / (float)width, y / (float)height);

                if (x < width && y < height)
                {
                    meshData.AddTriangles(vertI, vertI + width + 1, vertI + width);
                    meshData.AddTriangles(vertI + width + 1, vertI, vertI + 1);
                }
            }

            vertI++;

        }

        return meshData;

    }

}

public class MeshData
{

    public Vector3[] vertices;

    public int[] triangle;

    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {

        vertices = new Vector3[(meshWidth + 1) * (meshHeight + 1) * 3];

        uvs = new Vector2[(meshWidth + 1) * (meshHeight + 1) * 3];

        triangle = new int[meshWidth * meshHeight * 6];

    }

    public void AddTriangles(int a, int b, int c)

    {

        triangle[triangleIndex] = a;

        triangle[triangleIndex + 1] = b;

        triangle[triangleIndex + 2] = c;

        triangleIndex += 3;

    }

    public Mesh CreateMesh()

    {

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.vertices = vertices;

        mesh.triangles = triangle;

        mesh.uv = uvs;

        mesh.RecalculateNormals();

        return mesh;

    }

}