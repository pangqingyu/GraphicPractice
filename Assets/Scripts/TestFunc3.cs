using System.Collections;
using UnityEngine;

public class TestFunc3 : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(TestFunction());
    }

    void DebugCheck(string message, bool result)
    {
        if (result)
            Debug.Log(message + " success");
        else
            Debug.LogError(message + " fail");
    }

    IEnumerator TestFunction()
    {
        bool flag;
        Vector3 vector3a, vector3b, vector3c;
        MyVector3 myVector3a, myVector3b, myVector3c;
        float value;

        #region Check Vector3 MyVector3

        vector3a = Random.insideUnitSphere;
        myVector3a = vector3a.ToMyVector3();
        vector3b = Random.insideUnitSphere;
        myVector3b = vector3b.ToMyVector3();

        flag = vector3a.MyEquals(myVector3a) && myVector3b.MyEquals(vector3b);
        DebugCheck("vector3 Equals myVector3", flag);

        vector3c = vector3a + vector3b;
        myVector3c = myVector3a + myVector3b;
        flag = vector3c.MyEquals(myVector3c) && myVector3c.MyEquals(vector3c);
        DebugCheck("myVector3 add", flag);

        vector3c = vector3a - vector3b;
        myVector3c = myVector3a - myVector3b;

        flag = vector3c.MyEquals(myVector3c) && myVector3c.MyEquals(vector3c);
        DebugCheck("myVector3 minus", flag);

        flag = Vector3.Dot(vector3a, vector3b) == MyVector3.Dot(myVector3a, myVector3b);
        DebugCheck("myVector3 Dot", flag);

        vector3c = Vector3.Cross(vector3a, vector3b);
        myVector3c = MyVector3.Cross(myVector3a, myVector3b);
        flag = vector3c.MyEquals(myVector3c) && myVector3c.MyEquals(vector3c);
        DebugCheck("myVector3 Cross", flag);

        value = Random.Range(0f, 1);
        vector3c = vector3a * value;
        myVector3c = myVector3a * value;
        flag = vector3c.MyEquals(myVector3c) && myVector3c.MyEquals(vector3c);
        DebugCheck("myVector3 multiply float", flag);

        vector3c = vector3b / value;
        myVector3c = myVector3b / value;
        flag = vector3c.MyEquals(myVector3c) && myVector3c.MyEquals(vector3c);
        DebugCheck("myVector3 divide float", flag);

        #endregion

        yield return null;

        #region Check Matrix4x4 MyMatrix4x4

        Matrix4x4 matrix4X4, matrix4X4T, matrix4X4R, matrix4x4S;
        MyMatrix4x4 myMatrix4X4, myMatrix4X4T, myMatrix4X4R, myMatrix4X4S;

        transform.position = Random.insideUnitSphere;
        transform.rotation = Random.rotation;
        transform.localScale = Vector3.one * Random.Range(0.1f, 10f);

        myVector3a = transform.position.ToMyVector3();
        myVector3b = transform.eulerAngles.ToMyVector3();
        myVector3c = transform.localScale.ToMyVector3();

        matrix4X4T = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
        myMatrix4X4T = MyMatrix4x4.FromPosition(myVector3a);
        flag = matrix4X4T.MyEquals(myMatrix4X4T) && myMatrix4X4T.MyEquals(matrix4X4T);
        DebugCheck("MyMatrix4x4 FromPosition", flag);

        matrix4X4R = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(transform.eulerAngles), Vector3.one);
        myMatrix4X4R = MyMatrix4x4.FromRotation(myVector3b);
        flag = matrix4X4R.MyEquals(myMatrix4X4R) && myMatrix4X4R.MyEquals(matrix4X4R);
        DebugCheck("MyMatrix4x4 FromRotation", flag);

        matrix4x4S = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, transform.localScale);
        myMatrix4X4S = MyMatrix4x4.FromScale(myVector3c);
        flag = matrix4x4S.MyEquals(myMatrix4X4S) && myMatrix4X4S.MyEquals(matrix4x4S);
        DebugCheck("MyMatrix4x4 FromScale", flag);

        matrix4X4 = transform.localToWorldMatrix;
        myMatrix4X4 = MyMatrix4x4.TRS(myVector3a, myVector3b, myVector3c);
        flag = matrix4X4.MyEquals(myMatrix4X4) && myMatrix4X4.MyEquals(matrix4X4);
        DebugCheck("MyMatrix4x4 TRS", flag);

        #endregion
    }
}
