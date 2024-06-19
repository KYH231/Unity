using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class KYH : MonoBehaviour
{
    public List<Vector3> vertices = new List<Vector3>();
    public Material polygonMaterial;
    public Texture2D polygonTexture;
    private Mesh mesh;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var vertex in vertices)
        {
            Gizmos.DrawCube(transform.position + vertex, Vector3.one * 0.1f);
        }
    }

    public void CreatePolygon()
    {
        mesh = new Mesh();

        if (vertices.Count == 3)
        {
            Vector3[] verts = vertices.ToArray();
            int[] tris = new int[] { 0, 1, 2 };
            Vector2[] uvs = new Vector2[]
            {
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(0, 0)
            };

            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.uv = uvs;
        }
        else if (vertices.Count == 4)
        {
            Vector3[] verts = vertices.ToArray();
            int[] tris = new int[] { 0, 1, 2, 0, 2, 3 };
            Vector2[] uvs = new Vector2[]
            {
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 0)
            };

            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.uv = uvs;
        }
        else
        {
            Debug.LogWarning("A polygon requires either 3 or 4 vertices.");
            return;
        }

        // UV 좌표를 오른쪽으로 90도 회전시키기
        RotateUV(mesh);

        mesh.RecalculateNormals();

        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        if (mf == null) mf = gameObject.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        if (mr == null) mr = gameObject.AddComponent<MeshRenderer>();

        if (polygonTexture != null)
        {
            polygonMaterial = new Material(Shader.Find("Standard"));
            polygonMaterial.mainTexture = polygonTexture;
        }

        if (polygonMaterial != null)
        {
            mr.material = polygonMaterial;
        }
    }

    private void RotateUV(Mesh mesh)
    {
        Vector2[] uvs = mesh.uv;
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(1 - uvs[i].y, uvs[i].x);
        }
        mesh.uv = uvs;
    }

    public void DeletePolygon()
    {
        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        if (mf != null)
        {
            DestroyImmediate(mf);
        }

        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            DestroyImmediate(mr);
        }

        mesh = null;
    }

    public void DeleteLastVertex()
    {
        if (vertices.Count > 0)
        {
            vertices.RemoveAt(vertices.Count - 1);
        }
    }

    public void SaveToFile(string path)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            foreach (var vertex in vertices)
            {
                writer.WriteLine($"{vertex.x},{vertex.y},{vertex.z}");
            }
        }
    }

    public void LoadFromFile(string path)
    {
        vertices.Clear();
        using (StreamReader reader = new StreamReader(path))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(',');
                if (values.Length == 3)
                {
                    float x = float.Parse(values[0]);
                    float y = float.Parse(values[1]);
                    float z = float.Parse(values[2]);
                    vertices.Add(new Vector3(x, y, z));
                }
            }
        }
        CreatePolygon();
    }
}
