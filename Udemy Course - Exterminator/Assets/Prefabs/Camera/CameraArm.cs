using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraArm : MonoBehaviour
{
    [SerializeField] float armLength;
    [SerializeField] Transform child;
    private void Update()
    {
        child.position = transform.position - child.forward * armLength; 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(child.position, transform.position);
    }
}
