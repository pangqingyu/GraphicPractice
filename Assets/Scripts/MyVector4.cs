public struct MyVector4
{
    public float x, y, z, w;

    public MyVector4(float x, float y, float z, float w)
    {
        this.x = x; this.y = y; this.z = z; this.w = w;
    }

    public MyVector4(MyVector3 v, float w)
    {
        x = v.x; y = v.y; z = v.z; this.w = w;
    }

    public static MyVector4 operator *(MyVector4 a, float d)
    {
        return new MyVector4(a.x * d, a.y * d, a.z * d, a.w * d);
    }

    public static MyVector4 operator /(MyVector4 a, float d)
    {
        return new MyVector4(a.x / d, a.y / d, a.z / d, a.w / d);
    }

    public static implicit operator MyVector4(MyVector3 v)
    {
        return new MyVector4(v.x, v.y, v.z, 0f);
    }

    public static implicit operator MyVector3(MyVector4 v)
    {
        return new MyVector3(v.x, v.y, v.z);
    }

    public MyVector3 ToPoint()
    {
        return new MyVector3(x / w, y / w, z / w);
    }
}
