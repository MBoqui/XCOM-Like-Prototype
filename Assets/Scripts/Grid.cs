using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GridSystem
{
    public class Grid
    {
        public Vector2Int gridSize { get; private set; }
        GridElement[,] gridArray;

        //constructor
        public Grid(Vector2Int gridSize)
        {
            this.gridSize = gridSize;
            gridArray = new GridElement[gridSize.x, gridSize.y];

            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    gridArray[i, j] = new GridElement (this, new Vector2Int(i, j));
                }
            }
        }

        public Vector2Int? GetGridPosition (Vector3 worldPosition)
        {
            if (!IsInGrid(worldPosition))
            {
                Debug.Log("tried to get a position outside of grid");
                return null;
            }

            int xPosition = Mathf.FloorToInt(worldPosition.x);
            int yPosition = Mathf.FloorToInt(worldPosition.z);

            return new Vector2Int(xPosition, yPosition);
        }

        public Vector3? GetWorldPosition (Vector2Int gridPosition)
        {
            if (!IsInGrid(gridPosition))
            {
                Debug.Log("tried to get a position outside of grid");
                return null;
            }

            float xPosition = gridPosition.x + 0.5f;
            float zPosition = gridPosition.y + 0.5f;

            return new Vector3(xPosition, 0, zPosition);
        }

        public bool TryAddObject(GameObject prefab, Vector2Int gridPosition, bool isObstacle = false)
        {
            GridElement element = GetGridElement(gridPosition);

            //if grid element not valid return false
            if (element == null) return false;
            if (!element.IsFree()) return false;

            //create object and set it to element
            GridObject newGridObject = GridObject.Create(this, prefab, gridPosition);

            if (newGridObject == null) return false;

            element.SetGridObject(newGridObject);
            element.SetIsBlocked(isObstacle);

            return true;
        }

        public bool TryAddObject(GameObject prefab, Vector3 worldPosition, bool isObstacle = false)
        {
            Vector2Int? gridPosition = GetGridPosition(worldPosition);
            if (gridPosition == null) return false;

            return TryAddObject(prefab, (Vector2Int)gridPosition, isObstacle);
        }

        public bool TryDestroyObject(Vector2Int gridPosition)
        {
            GridElement element = GetGridElement(gridPosition);

            //if grid element not valid return false
            if (element == null) return false;
            if (element.IsFree()) return false;

            //if grid element valid, destroy object
            element.DestroyGridObject();
            return true;
        }

        public bool TryDestroyObject(Vector3 worldPosition)
        {
            Vector2Int? gridPosition = GetGridPosition(worldPosition);
            if (gridPosition == null) return false;

            return TryDestroyObject((Vector2Int)gridPosition);
        }

        public GridElement GetGridElement(Vector2Int gridPosition)
        {
            if (!IsInGrid(gridPosition)) return null;

            return gridArray[gridPosition.x, gridPosition.y];
        }

        public bool IsInGrid(Vector2Int gridPosition)
        {
            if (gridPosition.x < 0 ||
                gridPosition.y < 0 ||
                gridPosition.x > gridSize.x ||
                gridPosition.y > gridSize.y)
            {
                return false;
            } else {
                return true;
            }
        }

        public bool IsInGrid(Vector3 worldPosition)
        {
            if (worldPosition.x < 0 ||
                worldPosition.z < 0 ||
                worldPosition.x > gridSize.x ||
                worldPosition.z > gridSize.y)
            {
                return false;
            } else {
                return true;
            }
        }
    }
}
