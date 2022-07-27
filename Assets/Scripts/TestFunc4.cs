using UnityEngine;

[RequireComponent(typeof(MyCamera), typeof(Camera))]
public class TestFunc4 : MonoBehaviour
{
    void Update()
    {
        transform.position = Random.onUnitSphere * Random.Range(0.1f, 10f);
        transform.rotation = Random.rotation;

        Matrix4x4 matrix4X4 = GetComponent<Camera>().worldToCameraMatrix;
        MyMatrix4x4 myMatrix4X4 = GetComponent<MyCamera>().GetViewMatrix();
        bool flag = matrix4X4.MyEquals(myMatrix4X4) && myMatrix4X4.MyEquals(matrix4X4);
        DebugCheck("MyCamera GetViewMatrix", flag);
    }

    void DebugCheck(string message, bool result)
    {
        if (result)
            Debug.Log(message + " success");
        else
            Debug.LogError(message + " fail");
    }
}
