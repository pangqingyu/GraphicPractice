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
}
