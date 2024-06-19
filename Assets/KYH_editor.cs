using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KYH))]
public class KYH_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        KYH script = (KYH)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Add Default Vertices"))
        {
            script.vertices.Clear();
            script.vertices.Add(new Vector3(-1, -1, 0));    // 첫 번째 꼭짓점
            script.vertices.Add(new Vector3(-1, 1, 0));   // 두 번째 꼭짓점
            script.vertices.Add(new Vector3(1, 1, 0));  // 세 번째 꼭짓점
            script.vertices.Add(new Vector3(1, -1, 0));   // 네 번째 꼭짓점
        }

        if (GUILayout.Button("Create Polygon"))
        {
            script.CreatePolygon();
        }

        if (GUILayout.Button("Delete Polygon"))
        {
            script.DeletePolygon();
        }

        if (GUILayout.Button("Delete Last Vertex"))
        {
            script.DeleteLastVertex();
        }

        if (GUILayout.Button("Save to File"))
        {
            string path = EditorUtility.SaveFilePanel("Save Vertices", "", "vertices.txt", "txt");
            if (!string.IsNullOrEmpty(path))
            {
                script.SaveToFile(path);
            }
        }

        if (GUILayout.Button("Load from File"))
        {
            string path = EditorUtility.OpenFilePanel("Load Vertices", "", "txt");
            if (!string.IsNullOrEmpty(path))
            {
                script.LoadFromFile(path);
            }
        }

        if (GUILayout.Button("Set Polygon Material"))
        {
            Material material = new Material(Shader.Find("Standard"));
            material.color = Color.green; // 원하는 색상으로 변경
            script.polygonMaterial = material;
            if (script.GetComponent<MeshRenderer>())
            {
                script.GetComponent<MeshRenderer>().material = material;
            }
        }

        script.polygonTexture = (Texture2D)EditorGUILayout.ObjectField("Polygon Texture", script.polygonTexture, typeof(Texture2D), false);

        if (GUILayout.Button("Set Polygon Texture"))
        {
            if (script.polygonTexture != null)
            {
                script.CreatePolygon();
            }
        }
    }
}
