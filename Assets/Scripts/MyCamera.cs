using UnityEngine;
using UnityEngine.UI;

public class MyCamera : MonoBehaviour
{
    public float nearClipPlane = 0.1f, farClipPlane = 10f;
    [Range(1, 179)]
    public float verticalFov = 60f;
    [SerializeField]
    int ScreenWidth = 800, ScreenHeight = 600;
    float Aspect => (float)ScreenWidth / ScreenHeight;

    [SerializeField]
    Texture2D src;
    [SerializeField]
    RawImage rawImage;

    void Awake()
    {
        rawImage.texture = src;
    }

    public MyMatrix4x4 GetViewMatrix()
    {
        MyMatrix4x4 result = MyMatrix4x4.identity;
        result.m22 = -1;
        return result
            * MyMatrix4x4.FromRotation(new MyVector3(0, 0, -transform.eulerAngles.z))
            * MyMatrix4x4.FromRotation(new MyVector3(-transform.eulerAngles.x, 0, 0))
            * MyMatrix4x4.FromRotation(new MyVector3(0, -transform.eulerAngles.y, 0))
            * MyMatrix4x4.FromPosition((-transform.position).ToMyVector3());
    }

    public MyMatrix4x4 GetProjectionMatrix()
    {
        MyMatrix4x4 result = MyMatrix4x4.identity;
        result.m11 = 1 / Mathf.Tan(verticalFov / 2 * Mathf.Deg2Rad);
        result.m00 = result.m11 / Aspect;
        result.m22 = -1 * (farClipPlane + nearClipPlane) / (farClipPlane - nearClipPlane);
        result.m23 = -2 * farClipPlane * nearClipPlane / (farClipPlane - nearClipPlane);
        result.m32 = -1;
        result.m33 = 0;
        return result;
    }
}
