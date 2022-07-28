public struct MyVector3
{
    public float x, y, z;

    public MyVector3(float x, float y, float z)
    {
        this.x = x; this.y = y; this.z = z;
    }

    private static readonly MyVector3 zeroVector = new MyVector3(0f, 0f, 0f);
    public static MyVector3 zero => zeroVector;

    private static readonly MyVector3 rightVector = new MyVector3(1f, 0f, 0f);
    public static MyVector3 right => rightVector;

    private static readonly MyVector3 upVector = new MyVector3(0f, 1f, 0f);
    public static MyVector3 up => upVector;

    private static readonly MyVector3 forwardVector = new MyVector3(0f, 0f, 1f);
    public static MyVector3 forward => forwardVector;

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

    public MyVector3 Normalize()
    {
        float magnitude = (float)System.Math.Sqrt(x * x + y * y + z * z);
        return new MyVector3(x / magnitude, y / magnitude, z / magnitude);
    }
}
