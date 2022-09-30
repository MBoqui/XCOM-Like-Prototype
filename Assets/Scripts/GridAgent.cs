using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class GridAgent : MonoBehaviour
    {
        //exposed
        [SerializeField] float moveSpeed = 3f;

        //cache
        new Transform transform;
        Grid grid;

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
            transform.LookAt(moveTargetPosition);
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


        public Vector2Int GetGridPosition()
        {
            Vector2Int? gridPosition = grid.GetGridPosition(transform.position);

            if (gridPosition == null) return Vector2Int.zero;

            return (Vector2Int)gridPosition;
        }
    }
}
