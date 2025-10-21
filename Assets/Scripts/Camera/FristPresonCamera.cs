using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FristPresonCamera : MonoBehaviour
{
    //화면 회전
    public Transform playerTransform;
    public float mounseSensitivity = 200f;
    public float clampAngle = 80f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mounseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mounseSensitivity * Time.deltaTime;


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -clampAngle, clampAngle);

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        playerTransform.Rotate(Vector3.up * mouseX);
    }


}
