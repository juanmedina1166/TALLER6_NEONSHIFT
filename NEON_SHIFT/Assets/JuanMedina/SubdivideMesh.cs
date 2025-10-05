using UnityEngine;

public class SubdivideMesh : MonoBehaviour
{
    [Range(1, 100)]
    public int subdivisions = 1;

    // Esto añade un botón en el menú contextual del componente en el Inspector.
    [ContextMenu("Subdivide Mesh")]
    void ApplySubdivision()
    {
        // Obtenemos la malla original
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh originalMesh = meshFilter.sharedMesh;

        if (originalMesh == null)
        {
            Debug.LogError("No se encontró una malla en el MeshFilter.");
            return;
        }

        // Creamos una nueva malla para no modificar la original permanentemente
        Mesh newMesh = new Mesh();
        newMesh.name = "Subdivided_" + originalMesh.name;

        // Leemos los datos de la malla original
        Vector3[] originalVertices = originalMesh.vertices;
        Vector2[] originalUVs = originalMesh.uv;
        int[] originalTriangles = originalMesh.triangles;
        Vector3[] originalNormals = originalMesh.normals;

        // Listas para guardar los nuevos datos
        var newVertices = new System.Collections.Generic.List<Vector3>();
        var newUVs = new System.Collections.Generic.List<Vector2>();
        var newNormals = new System.Collections.Generic.List<Vector3>();
        var newTriangles = new System.Collections.Generic.List<int>();

        // --- Lógica de Subdivisión ---
        for (int i = 0; i < originalTriangles.Length; i += 3)
        {
            // Vértices del triángulo actual
            Vector3 v1 = originalVertices[originalTriangles[i]];
            Vector3 v2 = originalVertices[originalTriangles[i + 1]];
            Vector3 v3 = originalVertices[originalTriangles[i + 2]];

            // UVs del triángulo actual
            Vector2 uv1 = originalUVs[originalTriangles[i]];
            Vector2 uv2 = originalUVs[originalTriangles[i + 1]];
            Vector2 uv3 = originalUVs[originalTriangles[i + 2]];

            // Normales del triángulo actual
            Vector3 n1 = originalNormals[originalTriangles[i]];
            Vector3 n2 = originalNormals[originalTriangles[i + 1]];
            Vector3 n3 = originalNormals[originalTriangles[i + 2]];

            int baseIndex = newVertices.Count;

            // Dividimos el triángulo recursivamente (versión simplificada)
            // Para este ejemplo, simplemente subdividimos a lo largo del lado más largo
            // Una implementación más robusta usaría un algoritmo más complejo, pero esto funciona para terrenos planos.
            for (int j = 0; j <= subdivisions; j++)
            {
                float t = (float)j / subdivisions;
                newVertices.Add(Vector3.Lerp(v1, v2, t));
                newUVs.Add(Vector2.Lerp(uv1, uv2, t));
                newNormals.Add(Vector3.Lerp(n1, n2, t));

                newVertices.Add(Vector3.Lerp(v1, v3, t));
                newUVs.Add(Vector2.Lerp(uv1, uv3, t));
                newNormals.Add(Vector3.Lerp(n1, n3, t));
            }

            for (int j = 0; j < subdivisions; j++)
            {
                int a = baseIndex + j * 2;
                int b = baseIndex + j * 2 + 1;
                int c = baseIndex + (j + 1) * 2;
                int d = baseIndex + (j + 1) * 2 + 1;

                newTriangles.Add(a); newTriangles.Add(b); newTriangles.Add(c);
                newTriangles.Add(c); newTriangles.Add(b); newTriangles.Add(d);
            }
        }

        newMesh.SetVertices(newVertices);
        newMesh.SetUVs(0, newUVs);
        newMesh.SetNormals(newNormals);
        newMesh.SetTriangles(newTriangles, 0);

        // Asignamos la nueva malla subdividida
        meshFilter.mesh = newMesh;
        Debug.Log("Malla subdividida aplicada a " + gameObject.name);
    }
}
