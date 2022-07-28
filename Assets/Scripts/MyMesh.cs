using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MyMesh : MonoBehaviour
{
    public MyVector3[] vertices;
    public int[] triangles;
    public MyVector2[] uvs;
    public MyVector3[] normals;

    void Awake()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        if (mesh != null)
        {
            vertices = new MyVector3[mesh.vertices.Length];
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                vertices[i] = mesh.vertices[i].ToMyVector3();
            }
            triangles = mesh.triangles;
            uvs = new MyVector2[mesh.uv.Length];
            for (int i = 0; i < mesh.uv.Length; i++)
            {
                uvs[i] = mesh.uv[i].ToMyVector2();
            }
            normals = new MyVector3[mesh.normals.Length];
            for (int i = 0; i < mesh.normals.Length; i++)
            {
                normals[i] = mesh.normals[i].ToMyVector3();
            }
        }
        else
        {
            vertices = new MyVector3[] { MyVector3.zero, MyVector3.up, MyVector3.right };
            triangles = new int[] { 0, 1, 2 };
            uvs = new MyVector2[] { new MyVector2(0f, 0f), new MyVector2(0f, 1f), new MyVector2(1f, 0f) };
            normals = new MyVector3[] { MyVector3.forward * -1, MyVector3.forward * -1, MyVector3.forward * -1 };

            mesh = new Mesh
            {
                vertices = new Vector3[] { Vector3.zero, Vector3.up, Vector3.right },
                triangles = triangles,
                uv = new Vector2[] { Vector2.zero, Vector2.up, Vector2.right },
                normals = new Vector3[] { Vector3.forward * -1, Vector3.forward * -1, Vector3.forward * -1 }
            };
            GetComponent<MeshFilter>().mesh = mesh;
        }
    }
}
