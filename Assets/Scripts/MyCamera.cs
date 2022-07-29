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
    [SerializeField]
    Light dLight;
    [SerializeField]
    Color _DiffuseColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField]
    Color _SpecularColor = new Color(0.7f, 0.7f, 0.7f, 1f);
    [SerializeField]
    Color _AmbientColor = new Color(0.1f, 0.1f, 0.1f, 1f);
    [SerializeField]
    float _Shininess = 10f;
    [SerializeField]
    bool drawWire = false;
    [SerializeField]
    bool ZTest = false;
    Color _LightColor;
    Texture2D target;
    float[][] Zbuffer;
    MyMatrix4x4 mvp;
    MyMatrix4x4 worldToObjectMatrix;

    void Awake()
    {
        target = new Texture2D(ScreenWidth, ScreenHeight, TextureFormat.RGBA32, false);
        Zbuffer = new float[ScreenWidth][];
        for (int i = 0; i < ScreenWidth; i++)
        {
            Zbuffer[i] = new float[ScreenHeight];
        }
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

    void DrawMesh()
    {
        worldToObjectMatrix = MyMatrix4x4.WorldToObject(myMesh.transform);
        for (int i = 0; i < myMesh.triangles.Length; i += 3)
        {
            MyTriangle myTriangle = new MyTriangle
            {
                p1 = new Vertex
                {
                    posInObjectSpace = new Appdata(myMesh.vertices[myMesh.triangles[i]],
                                                   myMesh.normals[myMesh.triangles[i]],
                                                   myMesh.uvs[myMesh.triangles[i]],
                                                   myMesh.colors[myMesh.triangles[i]])
                },
                p2 = new Vertex
                {
                    posInObjectSpace = new Appdata(myMesh.vertices[myMesh.triangles[i + 1]],
                                                   myMesh.normals[myMesh.triangles[i + 1]],
                                                   myMesh.uvs[myMesh.triangles[i + 1]],
                                                   myMesh.colors[myMesh.triangles[i + 1]])
                },
                p3 = new Vertex
                {
                    posInObjectSpace = new Appdata(myMesh.vertices[myMesh.triangles[i + 2]],
                                                   myMesh.normals[myMesh.triangles[i + 2]],
                                                   myMesh.uvs[myMesh.triangles[i + 2]],
                                                   myMesh.colors[myMesh.triangles[i + 2]])
                }
            };
            if (drawWire)
                DrawWireTriangle(myTriangle);
            else
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
                    Color color = Color.Lerp(vertex1.posInClipSpace.color, vertex2.posInClipSpace.color, t);
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
                    Color color = Color.Lerp(vertex1.posInClipSpace.color, vertex2.posInClipSpace.color, t);
                    target.SetPixel(i, yInt, color);
                }
            }
        }
    }

    void DrawScanLine(ScanLine line)
    {
        if (0 <= line.y && line.y < ScreenHeight)
        {
            for (int i = Mathf.Max(line.left, 0); i <= Mathf.Min(line.right, ScreenWidth - 1); i++)
            {
                float t = Mathf.InverseLerp(line.left, line.right, i);
                float rhw = Mathf.Lerp(line.leftVertex.rhw, line.rightVertex.rhw, t);
                if (!ZTest || rhw >= Zbuffer[i][line.y])
                {
                    Color color = Color.Lerp(line.leftVertex.posInClipSpace.color, line.rightVertex.posInClipSpace.color, t);
                    target.SetPixel(i, line.y, color / rhw);
                    Zbuffer[i][line.y] = rhw;
                }
            }
        }
    }

    void InitDrawTriangle(MyTriangle triangle)
    {
        triangle.p1.posInClipSpace = Vert(triangle.p1.posInObjectSpace);
        triangle.p2.posInClipSpace = Vert(triangle.p2.posInObjectSpace);
        triangle.p3.posInClipSpace = Vert(triangle.p3.posInObjectSpace);

        triangle.p1.posInScreenSpace = CalPosInScreenSpace(triangle.p1.posInClipSpace.vertex);
        triangle.p2.posInScreenSpace = CalPosInScreenSpace(triangle.p2.posInClipSpace.vertex);
        triangle.p3.posInScreenSpace = CalPosInScreenSpace(triangle.p3.posInClipSpace.vertex);

        triangle.p1.Initrhw();
        triangle.p2.Initrhw();
        triangle.p3.Initrhw();
    }

    void DrawWireTriangle(MyTriangle triangle)
    {
        InitDrawTriangle(triangle);

        DrawLine(triangle.p1, triangle.p2);
        DrawLine(triangle.p2, triangle.p3);
        DrawLine(triangle.p3, triangle.p1);
    }

    void DrawTriangle(MyTriangle triangle)
    {
        InitDrawTriangle(triangle);

        MyVector3 v12 = triangle.p1.posInScreenSpace;
        v12 -= (MyVector3)triangle.p2.posInScreenSpace;
        MyVector3 v13 = triangle.p1.posInScreenSpace;
        v13 -= (MyVector3)triangle.p3.posInScreenSpace;
        MyVector3 n = MyVector3.Cross(v12, v13);
        //if (n.z > 0f)
        //    return;

        Vertex temp;
        // swap p1 and p2 if p1 is higher than p2
        if (triangle.p1.posInScreenSpace.y > triangle.p2.posInScreenSpace.y)
        {
            temp = triangle.p1;
            triangle.p1 = triangle.p2;
            triangle.p2 = temp;
        }
        // swap p1 and p3 if p1 is higher than p3
        if (triangle.p1.posInScreenSpace.y > triangle.p3.posInScreenSpace.y)
        {
            temp = triangle.p1;
            triangle.p1 = triangle.p3;
            triangle.p3 = temp;
        }
        // swap p2 and p3 if p2 is higher than p3
        if (triangle.p2.posInScreenSpace.y > triangle.p3.posInScreenSpace.y)
        {
            temp = triangle.p2;
            triangle.p2 = triangle.p3;
            triangle.p3 = temp;
        }

        ScanLine line = new ScanLine();
        int top = Mathf.RoundToInt(triangle.p3.posInScreenSpace.y);
        int middle = Mathf.RoundToInt(triangle.p2.posInScreenSpace.y);
        int bottom = Mathf.RoundToInt(triangle.p1.posInScreenSpace.y);
        float t;

        // single line
        if (top == bottom)
        {
            line.left = Mathf.RoundToInt(Mathf.Min(triangle.p1.posInScreenSpace.x, triangle.p2.posInScreenSpace.x, triangle.p3.posInScreenSpace.x));
            line.y = top;
            line.right = Mathf.RoundToInt(Mathf.Max(triangle.p1.posInScreenSpace.x, triangle.p2.posInScreenSpace.x, triangle.p3.posInScreenSpace.x));
            //DrawScanLine(line);
        }
        else
        {
            int x12, x13, x23;
            for (int i = bottom; i <= top; i++)
            {
                line.y = i;
                t = Mathf.InverseLerp(triangle.p1.posInScreenSpace.y, triangle.p2.posInScreenSpace.y, i);
                x12 = Mathf.RoundToInt(Mathf.Lerp(triangle.p1.posInScreenSpace.x, triangle.p2.posInScreenSpace.x, t));
                Vertex p12 = Vertex.Lerp(triangle.p1, triangle.p2, t);
                t = Mathf.InverseLerp(triangle.p1.posInScreenSpace.y, triangle.p3.posInScreenSpace.y, i);
                x13 = Mathf.RoundToInt(Mathf.Lerp(triangle.p1.posInScreenSpace.x, triangle.p3.posInScreenSpace.x, t));
                Vertex p13 = Vertex.Lerp(triangle.p1, triangle.p3, t);
                t = Mathf.InverseLerp(triangle.p2.posInScreenSpace.y, triangle.p3.posInScreenSpace.y, i);
                x23 = Mathf.RoundToInt(Mathf.Lerp(triangle.p2.posInScreenSpace.x, triangle.p3.posInScreenSpace.x, t));
                Vertex p23 = Vertex.Lerp(triangle.p2, triangle.p3, t);

                if (i < middle)
                {
                    if (x12 < x13)
                    {
                        line.left = x12;
                        line.right = x13;
                        line.leftVertex = p12;
                        line.rightVertex = p13;
                    }
                    else
                    {
                        line.left = x13;
                        line.right = x12;
                        line.leftVertex = p13;
                        line.rightVertex = p12;
                    }
                }
                else
                {
                    if (x13 < x23)
                    {
                        line.left = x13;
                        line.right = x23;
                        line.leftVertex = p13;
                        line.rightVertex = p23;
                    }
                    else
                    {
                        line.left = x23;
                        line.right = x13;
                        line.leftVertex = p23;
                        line.rightVertex = p13;
                    }
                }
                DrawScanLine(line);
            }
        }
    }

    MyVector3 UnityObjectToWorldNormal(MyVector3 normal)
    {
        MyVector3 result;
        result.x = normal.x * worldToObjectMatrix.m00 + normal.y * worldToObjectMatrix.m10 + normal.z * worldToObjectMatrix.m20;
        result.y = normal.x * worldToObjectMatrix.m01 + normal.y * worldToObjectMatrix.m11 + normal.z * worldToObjectMatrix.m21;
        result.z = normal.x * worldToObjectMatrix.m02 + normal.y * worldToObjectMatrix.m12 + normal.z * worldToObjectMatrix.m22;
        return result.Normalize();
    }

    MyVector3 WorldSpaceLightDir(MyVector4 vertex)
    {
        MyVector3 lightDir;
        MyVector3 angles = (dLight.transform.eulerAngles * Mathf.Deg2Rad).ToMyVector3();
        lightDir.x = Mathf.Cos(angles.x) * Mathf.Sin(angles.y) * -1;
        lightDir.y = Mathf.Sin(angles.x);
        lightDir.z = Mathf.Cos(angles.x) * Mathf.Cos(angles.y) * -1;
        return lightDir;
    }

    MyVector3 WorldSpaceViewDir(MyVector4 vertex)
    {
        MyVector3 viewDir = transform.position.ToMyVector3() - (MyVector3)(MyMatrix4x4.ObjectToWorld(myMesh.transform) * vertex);
        return viewDir.Normalize();
    }

    V2f Vert(Appdata i)
    {
        V2f output = new V2f();
        output.vertex = mvp * i.vertex;
        MyVector3 N = UnityObjectToWorldNormal(i.normal);
        MyVector3 L = WorldSpaceLightDir(i.vertex);
        MyVector3 V = WorldSpaceViewDir(i.vertex);
        MyVector3 H = ((L + V) / 2).Normalize();
        output.uv = i.uv;
        Color _colord = Mathf.Max(MyVector3.Dot(L, N), 0) * _DiffuseColor * _LightColor;
        Color _colora = _AmbientColor * _LightColor;
        Color _colors = Mathf.Pow(Mathf.Max(MyVector3.Dot(N, H), 0), _Shininess) * _SpecularColor * _LightColor;
        output.color = _colord + _colora + _colors;
        return output;
    }

    void Update()
    {
        _LightColor = dLight.color;
        mvp = GetProjectionMatrix() * GetViewMatrix() * MyMatrix4x4.ObjectToWorld(myMesh.transform);
        ClearScreen();
        DrawMesh();
        //DrawPoint(new Vertex
        //{
        //    posInObjectSpace = new Appdata(MyVector3.zero, new MyVector3(), new MyVector2())
        //});
        target.Apply();
    }

    void ClearScreen()
    {
        for (int i = 0; i < ScreenWidth; i++)
        {
            for (int j = 0; j < ScreenHeight; j++)
            {
                Zbuffer[i][j] = 0;
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
