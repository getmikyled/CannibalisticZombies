using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelatedCamera : MonoBehaviour
{
    [SerializeField] Camera pixelatedCamera;

    private void Awake()
    {
        pixelatedCamera.Render();
        pixelatedCamera.clearFlags = CameraClearFlags.Nothing;
    }
}
