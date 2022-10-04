using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;


namespace GridSystem
{
    public class Grid
    {
        public Vector2Int gridSize { get; private set; }
        GridElement[,] gridArray;
        PathFinder pathFinder;


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

            pathFinder = new PathFinder(this);
        }


        //public Methods
        //positional 
        public Vector2Int? GetGridPosition (Vector3 worldPosition)
        {
            if (!IsInGrid(worldPosition)) return null;

            int xPosition = Mathf.FloorToInt(worldPosition.x);
            int yPosition = Mathf.FloorToInt(worldPosition.z);

            return new Vector2Int(xPosition, yPosition);
        }


        public Vector3? GetWorldPosition (Vector2Int gridPosition)
        {
            if (!IsInGrid(gridPosition)) return null;

            float xPosition = gridPosition.x + 0.5f;
            float zPosition = gridPosition.y + 0.5f;

            return new Vector3(xPosition, 0, zPosition);
        }


        public bool IsInGrid(Vector2Int gridPosition)
        {
            if (gridPosition.x < 0 ||
                gridPosition.y < 0 ||
                gridPosition.x >= gridSize.x ||
                gridPosition.y >= gridSize.y)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public bool IsInGrid(Vector3 worldPosition)
        {
            if (worldPosition.x < 0 ||
                worldPosition.z < 0 ||
                worldPosition.x >= gridSize.x ||
                worldPosition.z >= gridSize.y)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        //content related
        public GridObject TryAddObject(GameObject prefab, Vector2Int gridPosition, bool isObstacle = false)
        {
            GridElement element = GetGridElement(gridPosition);

            //if grid element not valid return false
            if (element == null) return null;
            if (!element.IsFree()) return null;

            //create object and set it to element
            GridObject newGridObject = GridObject.Create(this, prefab, element, isObstacle);

            if (newGridObject == null) return null;

            element.SetGridObject(newGridObject);

            return newGridObject;
        }


        public GridObject TryAddObject(GameObject prefab, Vector3 worldPosition, bool isObstacle = false)
        {
            Vector2Int? gridPosition = GetGridPosition(worldPosition);
            if (gridPosition == null) return null;

            return TryAddObject(prefab, (Vector2Int)gridPosition, isObstacle);
        }


        public GridElement GetGridElement(Vector2Int gridPosition)
        {
            if (!IsInGrid(gridPosition)) return null;

            return gridArray[gridPosition.x, gridPosition.y];
        }


        public Vector2Int GetRandomPosition()
        {
            int x = Random.Range(0, gridSize.x);
            int y = Random.Range(0, gridSize.y);

            return new Vector2Int(x, y);
        }


        public Vector2Int FindAvailablePositionAround(Vector2Int center) //function that spirals around a point to find free positions
        {
            int minX = center.x;
            int maxX = center.x; 
            int minY = center.y;
            int maxY = center.y;

            Vector2Int current = center;

            while (true)
            {
                //check if is valid answer
                GridElement element = GetGridElement(current);
                Debug.Log(current);

                if (element != null) {
                    if (element.IsFree() && !element.IsBlocked())
                    {
                        return current;
                    }
                }

                //find next position in spiral
                if (current.x == maxX && current.y != maxY) //is at right of spiral
                {
                    if (current.y < minY) //is at edge
                    {
                        minY = current.y; //mark new min
                        current = new Vector2Int(current.x - 1, current.y); //move left
                    }
                    else //not on edge
                    {
                        current = new Vector2Int(current.x, current.y - 1); //move down
                    }
                }
                else if (current.y == minY) //is at bottom of spiral
                {
                    if (current.x < minX) //is at edge
                    {
                        minX = current.x; //mark new min
                        current = new Vector2Int(current.x, current.y + 1); //move up
                    }
                    else //not on edge
                    {
                        current = new Vector2Int(current.x - 1, current.y); //move left
                    }
                }
                else if (current.x == minX) //is at left of spiral
                {
                    if (current.y > maxY) //is at edge
                    {
                        maxY = current.y; //mark new max
                        current = new Vector2Int(current.x + 1, current.y); //move right
                    }
                    else //not on edge
                    {
                        current = new Vector2Int(current.x, current.y + 1); //move up
                    }
                }
                else if (current.y == maxY) //is at top of spiral
                {
                    if (current.x > maxX) //is at edge
                    {
                        maxX = current.x; //mark new max
                        current = new Vector2Int(current.x, current.y - 1); //move down
                    }
                    else //not on edge
                    {
                        current = new Vector2Int(current.x + 1, current.y); //move right
                    }
                }
            }
        }


        //pathfinding
        public (int, List<Vector2Int>) FindPath(Vector2Int startPosition, Vector2Int endPosition)
        {
            return pathFinder.FindPath(startPosition, endPosition);
        }
    }
}
