using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Interact,
    Idle,
    Move,
    Running,
    Attacking,
    Dead,
}

public class PlayerController : MonoBehaviour
{
    //플레이어
    public float moveSpeed = 10f;
    public float runSpeed = 20f;

    public float cameraRotationSpeed = 5f;

    private KeyCode inventoryKey = KeyCode.Tab;
    private KeyCode runKey = KeyCode.LeftShift;
    private float currentSpeed;
    private CharacterController controller;

    //카메라
    public CinemachineVirtualCamera playerCamera;
    public float mounseSensitivity = 1f;
    private float xRotation = 0f;
    private float yRotation = 0f;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = moveSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMove();
        RotationCamera();
    }

    void HandleMove()
    {
        if (Input.GetKey(runKey))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    void RotationCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mounseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mounseSensitivity;


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        yRotation += mouseX;

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

}
