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
    RawImage rawImage;
    Texture2D target;

    void Awake()
    {
        target = new Texture2D(ScreenWidth, ScreenHeight, TextureFormat.RGBA32, false);
        rawImage.texture = target;
        if (TryGetComponent<Camera>(out var cam))
        {
            cam.enabled = false;
            cam.orthographic = false;
            cam.nearClipPlane = nearClipPlane;
            cam.farClipPlane = farClipPlane;
            cam.fieldOfView = verticalFov;
        }
    }

    void Update()
    {
        Color color = Random.ColorHSV();
        for (int i = 0; i < ScreenWidth; i++)
        {
            for (int j = 0; j < ScreenHeight; j++)
            {
                target.SetPixel(i, j, color);
            }
        }
        target.Apply();
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
