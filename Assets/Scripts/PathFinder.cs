using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class PathFinder
    {
        const int DIAGONAL_COST = 3;
        const int STRAIGHT_COST = 2;

        Grid grid;

        List<GridElement> openList;
        List<GridElement> closedList;

        public PathFinder (Grid grid)
        {
            this.grid = grid;
        }

        public List<Vector2Int> FindPath(Vector2Int startPosition, Vector2Int endPosition)
        {
            //reset pathfinding values in all grid
            for (int i = 0; i < grid.gridSize.x; i++)
            {
                for (int j = 0; j < grid.gridSize.y; j++)
                {
                    grid.GetGridElement(new Vector2Int(i, j)).ResetPathInfo();
                }
            }

            //setup start of search
            GridElement startElement = grid.GetGridElement(startPosition);
            GridElement endElement = grid.GetGridElement(endPosition);

            if (startElement == null || endElement == null) return null; //invalid query

            openList = new List<GridElement> { startElement };
            closedList = new List<GridElement>();

            startElement.gCost = 0;
            startElement.hCost = CalculateHCost(startPosition, endPosition);
            startElement.CalculateFCost();

            //search
            while (openList.Count > 0)
            {
                GridElement currentElement = GetLowestFCostElement(openList);

                if (currentElement == endElement)
                {
                    return CalculatePath(endElement); //found path
                }

                //update lists
                openList.Remove(currentElement);
                closedList.Add(currentElement);

                foreach (GridElement neighbourElement in currentElement.GetNeighbourList())
                {
                    if (closedList.Contains(neighbourElement)) continue;
                    if (neighbourElement.isBlocked)
                    {
                        closedList.Add(neighbourElement);
                        continue;
                    }

                    //if gCost better than previously thought, update element path info
                    int newGCost = currentElement.gCost + CalculateTravelCost(currentElement, neighbourElement);
                    if (newGCost < neighbourElement.gCost)
                    {
                        neighbourElement.gCost = newGCost;
                        neighbourElement.parentElement = currentElement;
                        neighbourElement.hCost = CalculateHCost(neighbourElement.gridLocation, endPosition);
                        neighbourElement.CalculateFCost();

                        if (!openList.Contains(neighbourElement))
                        {
                            openList.Add(neighbourElement);
                        }
                    }
                }
            }

            return null; //path not found
        }

        int CalculateTravelCost(GridElement fromElement, GridElement toElement)
        {
            //temporarily the same thing, change when there is cost difference between terrains
            return CalculateHCost(fromElement.gridLocation, toElement.gridLocation);
        }

        int CalculateHCost (Vector2Int fromPosition, Vector2Int toPosition)
        {
            int deltaX = Mathf.Abs(fromPosition.x - toPosition.x);
            int deltaY = Mathf.Abs(fromPosition.y - toPosition.y);
            int diagonalMove = Mathf.Min(deltaX, deltaY);
            int straightMove = Mathf.Max(deltaX, deltaY) - diagonalMove;

            return diagonalMove * DIAGONAL_COST + straightMove * STRAIGHT_COST;
        }

        GridElement GetLowestFCostElement (List<GridElement> elementList)
        {
            GridElement lowestFCostElement = elementList[0];
            for (int i = 1; i < elementList.Count; i++)
            {
                if (elementList[i].fCost < lowestFCostElement.fCost)
                {
                    lowestFCostElement = elementList[i];
                }
            }
            return lowestFCostElement;
        }

        List<Vector2Int> CalculatePath(GridElement endElement)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            path.Add(endElement.gridLocation);

            GridElement currentElement = endElement;

            while (currentElement.parentElement != null)
            {
                path.Add(currentElement.parentElement.gridLocation);
                currentElement = currentElement.parentElement;
            }

            path.Reverse();

            return path;
        }
    }
}
