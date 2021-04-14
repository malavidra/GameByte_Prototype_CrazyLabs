using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    private Transform cam;
    [Range(0f, 30f)] public float _cameraP;

    private void Start()
    {
        cam = gameObject.transform;
    }

    public void TiltCamera(float newPos)
    {
        cam.eulerAngles = new Vector3(newPos,0,0);
    }
}
