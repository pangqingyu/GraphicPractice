using UnityEngine;
using UnityEngine.UI;

public class MyCamera : MonoBehaviour
{
    public float nearClipPlane = 0.1f, farClipPlane = 10f;
    [Range(1, 179)]
    public float verticalFov = 60f;
    [SerializeField]
    int ScreenWidth = 800, ScreenHeight = 600;
    float Aspect => (float)ScreenWidth / ScreenHeight;

    [SerializeField]
    RawImage rawImage;
    [SerializeField]
    Color clearColor = Color.gray;
    [SerializeField]
    MyMesh myMesh;

    Texture2D target;
    MyMatrix4x4 mvp;

    void Awake()
    {
        target = new Texture2D(ScreenWidth, ScreenHeight, TextureFormat.RGBA32, false);
        rawImage.texture = target;
        if (TryGetComponent<Camera>(out var cam))
        {
            cam.enabled = false;
            cam.orthographic = false;
            cam.nearClipPlane = nearClipPlane;
            cam.farClipPlane = farClipPlane;
            cam.fieldOfView = verticalFov;
        }
    }

    bool CheckVertex(Vertex vertex)
    {
        MyVector4 v = vertex.posInClipSpace.vertex;
        return -v.w < v.x && v.x < v.w
            && -v.w < v.y && v.y < v.w
            && 0 < v.z && v.z < v.w;
    }

    bool CheckLine(Vertex vertex1, Vertex vertex2)
    {
        return CheckVertex(vertex1)
            || CheckVertex(vertex2);
    }

    bool CheckTriangle(MyTriangle triangle)
    {
        return CheckVertex(triangle.p1)
            || CheckVertex(triangle.p2)
            || CheckVertex(triangle.p3);
    }

    void DrawMesh()
    {
        for (int i = 0; i < myMesh.triangles.Length; i += 3)
        {
            MyTriangle myTriangle = new MyTriangle
            {
                p1 = new Vertex
                {
                    posInObjectSpace = new Appdata(myMesh.vertices[myMesh.triangles[i]], myMesh.uvs[myMesh.triangles[i]])
                },
                p2 = new Vertex
                {
                    posInObjectSpace = new Appdata(myMesh.vertices[myMesh.triangles[i + 1]], myMesh.uvs[myMesh.triangles[i + 1]])
                },
                p3 = new Vertex
                {
                    posInObjectSpace = new Appdata(myMesh.vertices[myMesh.triangles[i + 2]], myMesh.uvs[myMesh.triangles[i + 2]])
                }
            };
            DrawTriangle(myTriangle);
        }
    }

    MyVector4 CalPosInScreenSpace(MyVector4 vertex)
    {
        vertex /= vertex.w;
        vertex.x = 0.5f * (vertex.x + 1) * ScreenWidth;
        vertex.y = 0.5f * (vertex.y + 1) * ScreenHeight;
        vertex.z = 0.5f * (farClipPlane - nearClipPlane) * vertex.z + 0.5f * (farClipPlane + nearClipPlane);
        return vertex;
    }

    void DrawPoint(Vertex p)
    {
        p.posInClipSpace = Vert(p.posInObjectSpace);
        if (CheckVertex(p))
        {
            p.posInScreenSpace = CalPosInScreenSpace(p.posInClipSpace.vertex);
            int x = Mathf.RoundToInt(p.posInScreenSpace.x);
            int y = Mathf.RoundToInt(p.posInScreenSpace.y);
            for (int i = 0; i < ScreenWidth; i++)
            {
                for (int j = 0; j < ScreenHeight; j++)
                {
                    if (Mathf.Abs(i - x) + Mathf.Abs(j - y) < 1)
                        target.SetPixel(i, j, Color.white);
                }
            }
        }
    }

    void DrawLine(Vertex vertex1, Vertex vertex2)
    {
        if (!CheckLine(vertex1, vertex2))
            return;

        int startXIndex = Mathf.RoundToInt(vertex1.posInScreenSpace.x);
        int endXIndex = Mathf.RoundToInt(vertex2.posInScreenSpace.x);
        int startYIndex = Mathf.RoundToInt(vertex1.posInScreenSpace.y);
        int endYIndex = Mathf.RoundToInt(vertex2.posInScreenSpace.y);

        float y21 = vertex2.posInScreenSpace.y - vertex1.posInScreenSpace.y;
        float x21 = vertex2.posInScreenSpace.x - vertex1.posInScreenSpace.x;
        float c = vertex1.posInScreenSpace.x * vertex2.posInScreenSpace.y
                - vertex2.posInScreenSpace.x * vertex1.posInScreenSpace.y;

        // false means -1 <= k <= 1
        bool flag = Mathf.Abs(y21) > Mathf.Abs(x21);
        // same point
        if (startXIndex == endXIndex && startYIndex == endYIndex)
        {
            DrawPoint(vertex1);
        }
        // vertical line
        else if (flag)
        {
            if (startYIndex > endYIndex)
            {
                // swap start end index
                startYIndex = endYIndex + 0 * (endYIndex = startYIndex);
            }
            for (int i = Mathf.Max(startYIndex, 0); i <= Mathf.Min(endYIndex, ScreenHeight - 1); i++)
            {
                float x = (x21 * i + c) / y21;
                int xInt = Mathf.RoundToInt(x);
                if (xInt >= 0 && xInt < ScreenWidth)
                {
                    float t = Mathf.InverseLerp(vertex1.posInScreenSpace.y, vertex2.posInScreenSpace.y, i);
                    Color color = Color.Lerp(vertex1.color, vertex2.color, t);
                    target.SetPixel(xInt, i, color);
                }
            }
        }
        else
        {
            if (startXIndex > endXIndex)
            {
                // swap start end index
                startXIndex = endXIndex + 0 * (endXIndex = startXIndex);
            }
            for (int i = Mathf.Max(startXIndex, 0); i <= Mathf.Min(endXIndex, ScreenWidth - 1); i++)
            {
                float y = (y21 * i - c) / x21;
                int yInt = Mathf.RoundToInt(y);
                if (yInt >= 0 && yInt < ScreenHeight)
                {
                    float t = Mathf.InverseLerp(vertex1.posInScreenSpace.x, vertex2.posInScreenSpace.x, i);
                    Color color = Color.Lerp(vertex1.color, vertex2.color, t);
                    target.SetPixel(i, yInt, color);
                }
            }
        }
    }

    void DrawTriangle(MyTriangle triangle)
    {
        triangle.p1.posInClipSpace = Vert(triangle.p1.posInObjectSpace);
        triangle.p2.posInClipSpace = Vert(triangle.p2.posInObjectSpace);
        triangle.p3.posInClipSpace = Vert(triangle.p3.posInObjectSpace);

        triangle.p1.posInScreenSpace = CalPosInScreenSpace(triangle.p1.posInClipSpace.vertex);
        triangle.p2.posInScreenSpace = CalPosInScreenSpace(triangle.p2.posInClipSpace.vertex);
        triangle.p3.posInScreenSpace = CalPosInScreenSpace(triangle.p3.posInClipSpace.vertex);

        triangle.p1.color = Random.ColorHSV();
        triangle.p2.color = Random.ColorHSV();
        triangle.p3.color = Random.ColorHSV();

        DrawLine(triangle.p1, triangle.p2);
        DrawLine(triangle.p2, triangle.p3);
        DrawLine(triangle.p3, triangle.p1);
    }

    V2f Vert(Appdata i)
    {
        V2f output = new V2f();
        output.vertex = mvp * i.vertex;
        output.uv = i.uv;
        return output;
    }

    void Update()
    {
        mvp = GetProjectionMatrix() * GetViewMatrix() * MyMatrix4x4.FromTransform(myMesh.transform);
        ClearScreen();
        DrawMesh();
        //DrawPoint(new Vertex
        //{
        //    posInObjectSpace = new Appdata(MyVector3.zero, new MyVector2())
        //});
        target.Apply();
    }

    void ClearScreen()
    {
        for (int i = 0; i < ScreenWidth; i++)
        {
            for (int j = 0; j < ScreenHeight; j++)
            {
                target.SetPixel(i, j, clearColor);
            }
        }
    }

    public MyMatrix4x4 GetViewMatrix()
    {
        MyMatrix4x4 result = MyMatrix4x4.identity;
        result.m22 = -1;
        return result
            * MyMatrix4x4.FromRotation(new MyVector3(0, 0, -transform.eulerAngles.z))
            * MyMatrix4x4.FromRotation(new MyVector3(-transform.eulerAngles.x, 0, 0))
            * MyMatrix4x4.FromRotation(new MyVector3(0, -transform.eulerAngles.y, 0))
            * MyMatrix4x4.FromPosition((-transform.position).ToMyVector3());
    }

    public MyMatrix4x4 GetProjectionMatrix()
    {
        MyMatrix4x4 result = MyMatrix4x4.identity;
        result.m11 = 1 / Mathf.Tan(verticalFov / 2 * Mathf.Deg2Rad);
        result.m00 = result.m11 / Aspect;
        result.m22 = -1 * (farClipPlane + nearClipPlane) / (farClipPlane - nearClipPlane);
        result.m23 = -2 * farClipPlane * nearClipPlane / (farClipPlane - nearClipPlane);
        result.m32 = -1;
        result.m33 = 0;
        return result;
    }
}
