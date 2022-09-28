using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GridSystem
{
    public class Grid
    {
        Vector2Int gridSize;
        Dictionary<Vector2Int, GridObject> grid = new Dictionary<Vector2Int, GridObject>();

        //constructor
        public Grid(Vector2Int gridSize)
        {
            this.gridSize = gridSize;
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
    }
}
