using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class Test : MonoBehaviour
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

        #endregion

        #region Check Vector4 MyVector4



        #endregion
        yield return null;
    }
}
