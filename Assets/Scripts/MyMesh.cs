using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MyMesh : MonoBehaviour
{
    MyVector3[] vertices;
    int[] triangles;

    void Awake()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        if (mesh != null)
        {
            vertices = new MyVector3[mesh.vertices.Length];
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                vertices[i] = mesh.vertices[i].ToMyVector3();
            }
            triangles = mesh.triangles;
        }
        else
        {
            vertices = new MyVector3[] { MyVector3.zero, MyVector3.right, MyVector3.up };
            triangles = new int[] { 0, 2, 1 };

            Vector3[] vertices2 = new Vector3[] { Vector3.zero, Vector3.right, Vector3.up };
            mesh = new Mesh
            {
                vertices = vertices2,
                triangles = triangles
            };
            GetComponent<MeshFilter>().mesh = mesh;
        }
    }
}
