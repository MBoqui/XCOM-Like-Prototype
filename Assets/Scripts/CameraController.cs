using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    Vector3 moveDirection;

    new Transform transform;

    void Awake()
    {
        transform = GetComponent<Transform>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        HandleInput();

        MoveCamera();
    }

    void MoveCamera()
    {
        transform.position += moveDirection * Time.deltaTime;
    }

    void HandleInput()
    {
        float xMove = 0;
        float zMove = 0;

        if (Input.GetKey(KeyCode.W))
        {
            zMove += 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            xMove -= 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            zMove -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            xMove += 1;
        }

        moveDirection = new Vector3(xMove, 0, zMove) * moveSpeed;
    }
}
