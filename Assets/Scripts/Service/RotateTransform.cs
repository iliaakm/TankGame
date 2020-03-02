using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTransform : MonoBehaviour
{
    public float lookSensitivity = 2;
    public float smoothDampTime = 0.1f;
    public bool allowZ, allowY;

    float zRotation;
    float yRotation;    
    float currZRotation;
    float currYRotation;
    float zRotationVelocity;
    float yRotationVelocity;
   
    void Update()
    {
        zRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;
        yRotation += Input.GetAxis("Mouse X") * lookSensitivity;

        zRotation = Mathf.Clamp(zRotation, -90, 90);

        if (allowZ)
            currZRotation = Mathf.SmoothDamp(currZRotation, zRotation, ref zRotationVelocity, smoothDampTime);
        else
            currZRotation = 0;

        if (allowY)
            currYRotation = Mathf.SmoothDamp(currYRotation, yRotation, ref yRotationVelocity, smoothDampTime);
        else
            currYRotation = 0;

        transform.localRotation = Quaternion.Euler(0, currYRotation, currZRotation);
    }
}
