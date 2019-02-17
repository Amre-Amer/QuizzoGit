using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCamMesh
{
    public Vector3[] points;
    public Mesh mesh;
    public Texture2D tex;
    public int resMeshX = 64;//*2; //64; //* 2; // = 10;
    public int resMeshY = 48; //*2; //48; //* 2; // = 30;
    public string meshShaderName = "Custom/DoubleSided";
    public string meshTextureName = "grid_opaque";
    public float grayscaleThreshold = .5f;
    public float meshHeight = 25; //10;
    public float radius = 10;
    public Vector3 center = Vector3.zero;
    MeshFilter meshFilter;
    int numY;
    GameObject goTmp;
    MeshRenderer meshRenderer;
    float ix;
    float iy;
    public MeshCamMesh()
    {
    }
    public GameObject CreateMeshGoFromTexture(Texture2D tex0)
    {
        //meshShaderName = "Standard";
        tex = tex0;
        //resMeshX = tex.width;
        //resMeshY = tex.height;
        Debug.Log(resMeshX + " x " + resMeshY + "\n");
        points = CreatePointsFromTexture();
        Debug.Log("points:" + points.Length + "\n");
        mesh = CreateMeshFromPoints();
        GameObject meshGo = new GameObject("meshGo:" + mesh.vertices.Length + " vertices\n");
        Debug.Log("meshGo:" + meshGo.name + "\n");
        meshFilter = meshGo.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = mesh;
        meshRenderer = meshGo.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find(meshShaderName));
        meshRenderer.sharedMaterial.mainTexture = Resources.Load<Texture2D>(meshTextureName);
        return meshGo;
    }

    public void UpdateMeshGoWithTexture(GameObject meshGo, Texture2D tex0) {
        float t1 = Time.realtimeSinceStartup;
        tex = tex0;
        CreatePointsFromTexture();
        mesh = CreateMeshFromPoints();
//        Debug.Log("meshGo:" + mesh.vertices.Length + " vertices\n");
        meshFilter.sharedMesh = mesh;
        meshRenderer.sharedMaterial.mainTexture = tex;
        float t2 = Time.realtimeSinceStartup - t1;
    }

    public Vector3[] CreatePointsFromTexture()
    {
        if (points == null) {
            points = new Vector3[resMeshX * resMeshY];
        }
        ix = tex.width / (float)resMeshX;
        iy = tex.height / (float)resMeshY;
//        Debug.Log("ix:" + ix + " iy:" + iy + "\n");
        //ix = 1;
        //iy = 1;
        for (int nx = 0; nx < resMeshX; nx++)
        {
            for (int ny = 0; ny < resMeshY; ny++)
            {
                int nPoints = nx * resMeshY + ny;
                float y = 0;
                int xx = nx * (int)Mathf.Round(ix);
                int yy = ny * (int)Mathf.Round(iy);
                //xx = nx;
                //yy = ny;
                Color color = tex.GetPixel(xx, yy);
                y = meshHeight * color.grayscale * 10;
//                y = meshHeight * (1 - color.grayscale) * 10;
                points[nPoints] = new Vector3(nx, y, ny);
//                Debug.Log("CreatePointsFormTexture:" + points[nPoints] +"\n");
            }
        }
        return points;
    }
    public Mesh CreateMeshFromPoints()
    {
        int numX = resMeshX - 1;
        numY = resMeshY - 1;
        int numFacesPerQuad = 2;
        int numVerticesPerQuad = 4;
        int numQuads = numX * numY;
        int numTrianglesPerQuad = numFacesPerQuad * 3;
        if (mesh == null)
        {
            mesh = new Mesh();
        }
//        Debug.Log("mesh numX:" + numX + " numY:" + numY + "\n");
        mesh.Clear();
        Vector3[] vertices = new Vector3[numVerticesPerQuad * numQuads];
        Vector2[] uvs = new Vector2[numVerticesPerQuad * numQuads];
        int[] triangles = new int[numTrianglesPerQuad * numQuads];
        int q = 0;
        for (int nx = 0; nx < numX; nx++)
        {
            for (int ny = 0; ny < numY; ny++)
            {
                int n0 = getDR2N(nx, ny);
                int n1 = getDR2N(nx, ny + 1);
                int n2 = getDR2N(nx + 1, ny + 1);
                int n3 = getDR2N(nx + 1, ny);
                Vector3 pnt0 = points[n0];
                Vector3 pnt1 = points[n1];
                Vector3 pnt2 = points[n2];
                Vector3 pnt3 = points[n3];
                //
                vertices[q * numVerticesPerQuad + 0] = pnt0;
                vertices[q * numVerticesPerQuad + 1] = pnt1;
                vertices[q * numVerticesPerQuad + 2] = pnt2;
                vertices[q * numVerticesPerQuad + 3] = pnt3;
                //
                float s = .05f;  // make bottom different texture uv from top
                if (pnt0.y != 0)
                {
                    s = .0125f;
                }
                float sx = (nx * ix) / (float) numX;
                float sy = (ny * iy) / (float) numY;
                sx = nx;
                sy = ny;
                sx = 1;
                sy = 1;
                float sx1 = (nx * ix) / (float)numX;
                float sy1 = (ny * iy) / (float)numY;
                float sx2 = sx1 + ix / (float)numX;
                float sy2 = sy1 + iy / (float)numY;
                //sx = pnt0.x;
                //sy = pnt0.z;
                //uvs[q * numVerticesPerQuad + 0] = new Vector2(0, 0);
                //uvs[q * numVerticesPerQuad + 1] = new Vector2(0, sy);
                //uvs[q * numVerticesPerQuad + 2] = new Vector2(sx, sy);
                //uvs[q * numVerticesPerQuad + 3] = new Vector2(sx, 0);
                //uvs[q * numVerticesPerQuad + 0] = new Vector2(sx1, sy1);
                //uvs[q * numVerticesPerQuad + 1] = new Vector2(sx1, sy2);
                //uvs[q * numVerticesPerQuad + 2] = new Vector2(sx2, sy2);
                //uvs[q * numVerticesPerQuad + 3] = new Vector2(sx2, sy1);
                float meshW = numX;
                float meshH = numY;
                uvs[q * numVerticesPerQuad + 0] = new Vector2(pnt0.x / numX, pnt0.z / numY);
                uvs[q * numVerticesPerQuad + 1] = new Vector2(pnt1.x / numX, pnt1.z / numY);
                uvs[q * numVerticesPerQuad + 2] = new Vector2(pnt2.x / numX, pnt2.z / numY);
                uvs[q * numVerticesPerQuad + 3] = new Vector2(pnt3.x / numX, pnt3.z / numY);
                //
                triangles[q * numTrianglesPerQuad + 0] = q * numVerticesPerQuad + 0;
                triangles[q * numTrianglesPerQuad + 1] = q * numVerticesPerQuad + 1;
                triangles[q * numTrianglesPerQuad + 2] = q * numVerticesPerQuad + 2;
                triangles[q * numTrianglesPerQuad + 3] = q * numVerticesPerQuad + 0;
                triangles[q * numTrianglesPerQuad + 4] = q * numVerticesPerQuad + 2;
                triangles[q * numTrianglesPerQuad + 5] = q * numVerticesPerQuad + 3;
                q++;
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }
    int getDR2N(int dx, int dy)
    {
        return dx * (numY + 1) + dy;
    }
}
