using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class GridAgent : MonoBehaviour
    {
        //exposed
        [SerializeField] float moveSpeed = 3f;
        [SerializeField] float rotateSpeed = 3f;
        public Vector2Int gridPosition { get => gridObject.gridPosition; }

        //cache
        new Transform transform;
        Grid grid;
        GridObject _gridObject;
        GridObject gridObject { get => GetGridObject(); }

        //move
        List<Vector3> movePath = new List<Vector3> ();
        int currentPathIndex;
        Vector3 moveTargetPosition;
        bool isRotating;


        void Awake()
        {
            transform = GetComponent<Transform>();
        }


        void Start()
        {
            grid = GetComponent<GridSystem.GridObject>().grid;
        }


        void Update()
        {
            HandleRotation();

            HandleMovement();
        }


        void HandleRotation()
        {
            if (movePath.Count <= 0) return;

            Vector3 targetDirection = moveTargetPosition - transform.position;
            Vector3 currentDirection = transform.forward;

            if (Vector3.Angle(targetDirection, currentDirection) < 0.5f)
            {
                isRotating = false;
                return;
            }
            isRotating = true;

            float step = rotateSpeed * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDirection);
        }


        void HandleMovement()
        {
            if (movePath.Count <= 0) return;
            if (isRotating) return;

            moveTargetPosition = movePath[currentPathIndex];
            float distance = Vector3.Distance(moveTargetPosition, transform.position);
            if (distance > 0.05f)
            {
                Vector3 moveDirection = (moveTargetPosition - transform.position).normalized;

                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            } else {
                currentPathIndex++;
                if (currentPathIndex >= movePath.Count)
                {
                    //move object in grid
                    Vector2Int newGridPosition = (Vector2Int)grid.GetGridPosition(transform.position);
                    GridElement newGridElement = grid.GetGridElement(newGridPosition);
                    newGridElement.SetGridObject(gridObject);

                    movePath.Clear();
                }
            }
        }


        public void SetMovePath(List<Vector2Int> path)
        {
            movePath.Clear();
            currentPathIndex = 0;
            foreach(Vector2Int gridPosition in path)
            {
                Vector3? worldPosition = grid.GetWorldPosition(gridPosition);

                if (worldPosition == null) continue;

                movePath.Add((Vector3)worldPosition);
            }
            movePath.RemoveAt(0);
        }


        GridObject GetGridObject()
        {
            if (_gridObject == null)
            {
                _gridObject = GetComponent<GridObject>();
            }

            return _gridObject;
        }
    }
}
