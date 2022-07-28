using UnityEngine;

public struct MyMatrix4x4
{
    public float m00, m01, m02, m03;
    public float m10, m11, m12, m13;
    public float m20, m21, m22, m23;
    public float m30, m31, m32, m33;

    private static readonly MyMatrix4x4 identityMatrix = new MyMatrix4x4(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);
    public static MyMatrix4x4 identity => identityMatrix;

    public MyMatrix4x4(float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30, float m31, float m32, float m33)
    {
        this.m00 = m00;
        this.m01 = m01;
        this.m02 = m02;
        this.m03 = m03;
        this.m10 = m10;
        this.m11 = m11;
        this.m12 = m12;
        this.m13 = m13;
        this.m20 = m20;
        this.m21 = m21;
        this.m22 = m22;
        this.m23 = m23;
        this.m30 = m30;
        this.m31 = m31;
        this.m32 = m32;
        this.m33 = m33;
    }

    public static MyMatrix4x4 FromScale(MyVector3 s)
    {
        MyMatrix4x4 result = identityMatrix;
        result.m00 = s.x;
        result.m11 = s.y;
        result.m22 = s.z;
        return result;
    }

    public static MyMatrix4x4 FromPosition(MyVector3 pos)
    {
        MyMatrix4x4 result = identityMatrix;
        result.m03 = pos.x;
        result.m13 = pos.y;
        result.m23 = pos.z;
        return result;
    }

    public static MyMatrix4x4 FromRotation(MyVector3 r)
    {
        r *= Mathf.Deg2Rad;
        MyMatrix4x4 rx = identityMatrix;
        rx.m11 = Mathf.Cos(r.x);
        rx.m12 = -Mathf.Sin(r.x);
        rx.m21 = Mathf.Sin(r.x);
        rx.m22 = Mathf.Cos(r.x);

        MyMatrix4x4 ry = identityMatrix;
        ry.m00 = Mathf.Cos(r.y);
        ry.m02 = Mathf.Sin(r.y);
        ry.m20 = -Mathf.Sin(r.y);
        ry.m22 = Mathf.Cos(r.y);

        MyMatrix4x4 rz = identityMatrix;
        rz.m00 = Mathf.Cos(r.z);
        rz.m01 = -Mathf.Sin(r.z);
        rz.m10 = Mathf.Sin(r.z);
        rz.m11 = Mathf.Cos(r.z);
        return ry * rx * rz;
    }

    public static MyMatrix4x4 TRS(MyVector3 pos, MyVector3 r, MyVector3 s)
    {
        return FromPosition(pos) * FromRotation(r) * FromScale(s);
    }

    public static MyMatrix4x4 ObjectToWorld(Transform t)
    {
        MyVector3 pos = t.position.ToMyVector3();
        MyVector3 r = t.eulerAngles.ToMyVector3();
        MyVector3 s = t.lossyScale.ToMyVector3();
        return TRS(pos, r, s);
    }

    public static MyMatrix4x4 WorldToObject(Transform t)
    {
        return FromScale(new MyVector3(1f / t.lossyScale.x, 1f / t.lossyScale.y, 1f / t.lossyScale.z))
             * FromRotation(new MyVector3(0, 0, -t.eulerAngles.z))
             * FromRotation(new MyVector3(-t.eulerAngles.x, 0, 0))
             * FromRotation(new MyVector3(0, -t.eulerAngles.y, 0))
             * FromPosition((-t.position).ToMyVector3());
    }

    public static MyMatrix4x4 operator *(MyMatrix4x4 lhs, MyMatrix4x4 rhs)
    {
        MyMatrix4x4 result;
        result.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30;
        result.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31;
        result.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32;
        result.m03 = lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33;
        result.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30;
        result.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31;
        result.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32;
        result.m13 = lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33;
        result.m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30;
        result.m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31;
        result.m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32;
        result.m23 = lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33;
        result.m30 = lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30;
        result.m31 = lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31;
        result.m32 = lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32;
        result.m33 = lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33;
        return result;
    }

    public static MyVector4 operator *(MyMatrix4x4 m, MyVector4 v)
    {
        MyVector4 result;
        result.x = m.m00 * v.x + m.m01 * v.y + m.m02 * v.z + m.m03 * v.w;
        result.y = m.m10 * v.x + m.m11 * v.y + m.m12 * v.z + m.m13 * v.w;
        result.z = m.m20 * v.x + m.m21 * v.y + m.m22 * v.z + m.m23 * v.w;
        result.w = m.m30 * v.x + m.m31 * v.y + m.m32 * v.z + m.m33 * v.w;
        return result;
    }

    public MyVector3 MultiplyPoint(MyVector3 p)
    {
        MyVector4 result = this * new MyVector4(p, 1f);
        return result.ToPoint();
    }

    public MyVector3 MultiplyVector(MyVector3 p)
    {
        MyVector4 result = this * new MyVector4(p, 0f);
        return result;
    }
}
