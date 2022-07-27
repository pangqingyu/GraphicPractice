using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyCamera : MonoBehaviour
{
    [SerializeField]
    float nearClipPlane, farClipPlane;
    [SerializeField, Range(1, 179)]
    float fov;
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
}
