using UnityEngine;

[RequireComponent(typeof(MyCamera), typeof(Camera))]
public class TestFunc4 : MonoBehaviour
{
    Camera camera;
    MyCamera myCamera;

    void Awake()
    {
        camera = GetComponent<Camera>();
        myCamera = GetComponent<MyCamera>();
    }

    void Update()
    {
        transform.position = Random.onUnitSphere * Random.Range(0.1f, 10f);
        transform.rotation = Random.rotation;

        Matrix4x4 matrix4X4 = camera.worldToCameraMatrix;
        MyMatrix4x4 myMatrix4X4 = myCamera.GetViewMatrix();
        bool flag = matrix4X4.MyEquals(myMatrix4X4) && myMatrix4X4.MyEquals(matrix4X4);
        DebugCheck("MyCamera GetViewMatrix", flag);

        camera.fieldOfView = Random.Range(1f, 179f);
        myCamera.verticalFov = camera.fieldOfView;
        camera.nearClipPlane = Random.Range(0.1f, 10f);
        myCamera.nearClipPlane = camera.nearClipPlane;
        camera.farClipPlane = Random.Range(1f, 100f) * camera.nearClipPlane;
        myCamera.farClipPlane = camera.farClipPlane;

        matrix4X4 = camera.projectionMatrix;
        myMatrix4X4 = myCamera.GetProjectionMatrix();
        flag = matrix4X4.MyEquals(myMatrix4X4) && myMatrix4X4.MyEquals(matrix4X4);
        DebugCheck("MyCamera GetProjectionMatrix", flag);
    }

    void DebugCheck(string message, bool result)
    {
        if (result)
            Debug.Log(message + " success");
        else
            Debug.LogError(message + " fail");
    }
}
