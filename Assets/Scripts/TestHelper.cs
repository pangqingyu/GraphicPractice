using UnityEngine;

public static class TestHelper
{
    #region Vector3

    public static MyVector3 ToMyVector3(this Vector3 vector3)
    {
        return new MyVector3(vector3.x, vector3.y, vector3.z);
    }

    public static Vector3 ToVector3(this MyVector3 vector3)
    {
        return new Vector3(vector3.x, vector3.y, vector3.z);
    }

    public static bool MyEquals(this Vector3 v1, MyVector3 v2)
    {
        return v1.Equals(v2.ToVector3());
    }

    public static bool MyEquals(this MyVector3 v1, Vector3 v2)
    {
        return v1.ToVector3().Equals(v2);
    }

    #endregion

    #region Vector4

    public static MyVector4 ToMyVector4(this Vector4 vector4)
    {
        return new MyVector4(vector4.x, vector4.y, vector4.z, vector4.w);
    }

    public static Vector4 ToVector4(this MyVector4 vector4)
    {
        return new Vector4(vector4.x, vector4.y, vector4.z, vector4.w);
    }

    public static bool MyEquals(this Vector4 v1, MyVector4 v2)
    {
        return v1.Equals(v2.ToVector4());
    }

    public static bool MyEquals(this MyVector4 v1, Vector4 v2)
    {
        return v1.ToVector4().Equals(v2);
    }

    #endregion

    public static MyMatrix4x4 ToMyMatrix4x4(this Matrix4x4 matrix4X4)
    {
        MyMatrix4x4 result;
        result.m00 = matrix4X4.m00;
        result.m01 = matrix4X4.m01;
        result.m02 = matrix4X4.m02;
        result.m03 = matrix4X4.m03;
        result.m10 = matrix4X4.m10;
        result.m11 = matrix4X4.m11;
        result.m12 = matrix4X4.m12;
        result.m13 = matrix4X4.m13;
        result.m20 = matrix4X4.m20;
        result.m21 = matrix4X4.m21;
        result.m22 = matrix4X4.m22;
        result.m23 = matrix4X4.m23;
        result.m30 = matrix4X4.m30;
        result.m31 = matrix4X4.m31;
        result.m32 = matrix4X4.m32;
        result.m33 = matrix4X4.m33;
        return result;
    }

    public static Matrix4x4 ToMatrix4x4(this MyMatrix4x4 matrix4X4)
    {
        Matrix4x4 result;
        result.m00 = matrix4X4.m00;
        result.m01 = matrix4X4.m01;
        result.m02 = matrix4X4.m02;
        result.m03 = matrix4X4.m03;
        result.m10 = matrix4X4.m10;
        result.m11 = matrix4X4.m11;
        result.m12 = matrix4X4.m12;
        result.m13 = matrix4X4.m13;
        result.m20 = matrix4X4.m20;
        result.m21 = matrix4X4.m21;
        result.m22 = matrix4X4.m22;
        result.m23 = matrix4X4.m23;
        result.m30 = matrix4X4.m30;
        result.m31 = matrix4X4.m31;
        result.m32 = matrix4X4.m32;
        result.m33 = matrix4X4.m33;
        return result;
    }

    public static bool Approximately(float a, float b)
    {
        return Mathf.Abs(a - b) < 1e-4f;
    }

    public static bool MyEquals(this Matrix4x4 m1, MyMatrix4x4 m2)
    {
        return Approximately(m1.m00, m2.m00)
            && Approximately(m1.m01, m2.m01)
            && Approximately(m1.m02, m2.m02)
            && Approximately(m1.m03, m2.m03)
            && Approximately(m1.m10, m2.m10)
            && Approximately(m1.m11, m2.m11)
            && Approximately(m1.m12, m2.m12)
            && Approximately(m1.m13, m2.m13)
            && Approximately(m1.m20, m2.m20)
            && Approximately(m1.m21, m2.m21)
            && Approximately(m1.m22, m2.m22)
            && Approximately(m1.m23, m2.m23)
            && Approximately(m1.m30, m2.m30)
            && Approximately(m1.m31, m2.m31)
            && Approximately(m1.m32, m2.m32)
            && Approximately(m1.m33, m2.m33);
    }

    public static bool MyEquals(this MyMatrix4x4 m1, Matrix4x4 m2)
    {
        return Approximately(m1.m00, m2.m00)
            && Approximately(m1.m01, m2.m01)
            && Approximately(m1.m02, m2.m02)
            && Approximately(m1.m03, m2.m03)
            && Approximately(m1.m10, m2.m10)
            && Approximately(m1.m11, m2.m11)
            && Approximately(m1.m12, m2.m12)
            && Approximately(m1.m13, m2.m13)
            && Approximately(m1.m20, m2.m20)
            && Approximately(m1.m21, m2.m21)
            && Approximately(m1.m22, m2.m22)
            && Approximately(m1.m23, m2.m23)
            && Approximately(m1.m30, m2.m30)
            && Approximately(m1.m31, m2.m31)
            && Approximately(m1.m32, m2.m32)
            && Approximately(m1.m33, m2.m33);
    }
}
