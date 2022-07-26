public struct MyVector3
{
    public float x, y, z;

    public MyVector3(float x, float y, float z)
    {
        this.x = x; this.y = y; this.z = z;
    }

    public static float Dot(MyVector3 lhs, MyVector3 rhs)
    {
        return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
    }

    public static MyVector3 Cross(MyVector3 lhs, MyVector3 rhs)
    {
        return new MyVector3(lhs.y * rhs.z - lhs.z * rhs.y,
                             lhs.z * rhs.x - lhs.x * rhs.z,
                             lhs.x * rhs.y - lhs.y * rhs.x);
    }

    public static MyVector3 operator +(MyVector3 a, MyVector3 b)
    {
        return new MyVector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static MyVector3 operator -(MyVector3 a, MyVector3 b)
    {
        return new MyVector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static MyVector3 operator *(MyVector3 a, float d)
    {
        return new MyVector3(a.x * d, a.y * d, a.z * d);
    }

    public static MyVector3 operator /(MyVector3 a, float d)
    {
        return new MyVector3(a.x / d, a.y / d, a.z / d);
    }
}
