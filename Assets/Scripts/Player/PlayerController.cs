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
    public float moveSpeed = 3f;
    public float runSpeed = 7f;

    public float cameraRotationSpeed = 5f;


    private KeyCode runKey = KeyCode.LeftShift;

    private float currentSpeed;
    private CharacterController controller;

    //카메라
    public CinemachineVirtualCamera playerCamera;

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


}
