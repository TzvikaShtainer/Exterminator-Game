using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTrans;
    [SerializeField] private float turnSpeed = 2f;
    private void LateUpdate()
    {
        transform.position = followTrans.position; 
    }

    public void AddYawInput(float amt) //add the rotation of the X 
    {
        transform.Rotate(Vector3.up, amt * Time.deltaTime * turnSpeed);
    }
}
