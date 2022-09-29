using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GridSystem
{
    public class Grid
    {
        Vector2Int gridSize;
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
                    gridArray[i, j] = new GridElement (i, j);
                }
            }
        }

        public Vector2Int? GetXY (Vector3 worldPosition)
        {
            if (worldPosition.x < 0 ||
                worldPosition.z < 0 ||
                worldPosition.x > gridSize.x ||
                worldPosition.z > gridSize.y)
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
            if (gridPosition.x < 0 ||
                gridPosition.y < 0 ||
                gridPosition.x > gridSize.x ||
                gridPosition.y > gridSize.y)
            {
                Debug.Log("tried to get a position outside of grid");
                return null;
            }

            float xPosition = gridPosition.x + 0.5f;
            float zPosition = gridPosition.y + 0.5f;

            return new Vector3(xPosition, 0, zPosition);
        }

        public bool TryAddObject(GameObject prefab, Vector2Int gridPosition)
        {
            GridObject newGridObject = GridObject.Create(this, prefab, gridPosition);

            if (newGridObject == null)
            {
                return false;
            } else {
                return true;
            }
        }

        public bool TryAddObject(GameObject prefab, Vector3 worldPosition)
        {
            Vector2Int? gridPosition = GetXY(worldPosition);
            if (gridPosition == null)
            {
                Debug.LogWarning("tried adding object outside of grid");
                return false;
            }

            return TryAddObject(prefab, (Vector2Int)gridPosition);
        }

        class GridElement
        {
            int x;
            int y;

            //TerrainType ...
            GridObject gridObject;

            public GridElement(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
    }
}
