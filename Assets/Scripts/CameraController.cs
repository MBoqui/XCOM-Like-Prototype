using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [SerializeField] float moveSpeed = 5;
    [SerializeField] float zOffset = 8;
    Vector3 moveDirection;

    new Transform transform;


    //Unity Messages
    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;

        transform = GetComponent<Transform>();
    }


    void Update()
    {
        HandleInput();

        MoveCamera();
    }


    //public Methods
    public void JumpTo(Vector3 position)
    {
        transform.position = position + new Vector3(0, transform.position.y, -zOffset);
    }


    //private Methods
    void MoveCamera()
    {
        Vector3 position = transform.position;
        Vector2Int gridSize = GameSettings.Instance.gridSize;

        Vector3 newPosition = position + moveDirection * Time.deltaTime;

        if (newPosition.x < 0 ||
            newPosition.z < - zOffset ||
            newPosition.x >= gridSize.x ||
            newPosition.z >= gridSize.y - zOffset) return;

        transform.position = newPosition;
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

        moveDirection = new Vector3(xMove, 0, zMove).normalized * moveSpeed;
    }
}
