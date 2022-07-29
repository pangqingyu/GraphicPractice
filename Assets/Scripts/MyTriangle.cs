using UnityEngine;

public struct MyVector2
{
    public float u, v;
    public MyVector2(float u, float v)
    {
        this.u = u; this.v = v;
    }

    public static MyVector2 operator *(MyVector2 a, float b)
    {
        return new MyVector2(a.u * b, a.v * b);
    }

    public static MyVector2 Lerp(MyVector2 a, MyVector2 b, float t)
    {
        return new MyVector2(a.u + (b.u - a.u) * t, a.v + (b.v - a.v) * t);
    }
};

public struct Appdata
{
    public MyVector4 vertex;
    public MyVector3 normal;
    public MyVector2 uv;
    public Color color;

    public Appdata(MyVector3 pos, MyVector3 normal, MyVector2 uv, Color color = new Color())
    {
        vertex = new MyVector4(pos, 1f);
        this.normal = normal;
        this.uv = uv;
        this.color = color;
    }
};

public struct V2f
{
    public MyVector4 vertex;
    public MyVector2 uv;
    public Color color;
    public static V2f Lerp(V2f a, V2f b, float t)
    {
        V2f v = new V2f();
        v.vertex = MyVector4.Lerp(a.vertex, b.vertex, t);
        v.uv = MyVector2.Lerp(a.uv, b.uv, t);
        v.color = Color.Lerp(a.color, b.color, t);
        return v;
    }
};

public struct ScanLine
{
    public int left, y, right;
    public Vertex leftVertex, rightVertex;
}

public class Vertex
{
    public Appdata posInObjectSpace;
    public V2f posInClipSpace;
    public MyVector4 posInScreenSpace;
    public float rhw;
    public static Vertex Lerp(Vertex a, Vertex b, float t)
    {
        Vertex v = new Vertex();
        v.posInClipSpace = V2f.Lerp(a.posInClipSpace, b.posInClipSpace, t);
        v.posInScreenSpace = MyVector4.Lerp(a.posInScreenSpace, b.posInScreenSpace, t);
        v.rhw = a.rhw + (b.rhw - a.rhw) * t;
        return v;
    }

    public void Initrhw()
    {
        rhw = 1f / posInClipSpace.vertex.w;
        posInClipSpace.uv *= rhw;
        posInClipSpace.color *= rhw;
    }
}

public class MyTriangle
{
    public Vertex p1, p2, p3;
}
