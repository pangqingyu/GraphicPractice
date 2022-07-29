using UnityEngine;

public struct MyVector2
{
    public float u, v;
    public MyVector2(float u, float v)
    {
        this.u = u; this.v = v;
    }
};

public struct Appdata
{
    public MyVector4 vertex;
    public MyVector3 normal;
    public MyVector2 uv;

    public Appdata(MyVector3 pos, MyVector3 normal, MyVector2 uv)
    {
        vertex = new MyVector4(pos, 1f);
        this.normal = normal;
        this.uv = uv;
    }
};

public struct V2f
{
    public MyVector4 vertex;
    public MyVector2 uv;
    public Color color;
};

public struct ScanLine
{
    public int left, y, right;
}

public class Vertex
{
    public Appdata posInObjectSpace;
    public V2f posInClipSpace;
    public MyVector4 posInScreenSpace;
}

public class MyTriangle
{
    public Vertex p1, p2, p3;
}
