using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class GridElement
    {
        //Element content
        Grid grid;
        public Vector2Int gridLocation { get; private set; }

        //TerrainType ...
        GridObject gridObject;

        //Pathfinding
        public int gCost;
        public int hCost;
        public int fCost;
        public bool isBlocked;
        public GridElement parentElement;


        public GridElement(Grid grid, Vector2Int gridLocation)
        {
            this.grid = grid;
            this.gridLocation = gridLocation;
        }


        public bool IsFree()
        {
            return gridObject == null;
        }


        public void SetGridObject(GridObject gridObject)
        {
            this.gridObject = gridObject;
            gridObject.SetGridLocation(gridLocation);
        }


        public void SetIsBlocked(bool value) 
        {
            isBlocked = value;
        }


        public void ClearGridObject()
        {
            gridObject = null;
            isBlocked = false;
        }


        public void DestroyGridObject()
        {
            gridObject.DestroySelf();
        }


        public void ResetPathInfo()
        {
            gCost = int.MaxValue;
            CalculateFCost();
            parentElement = null;
        }


        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }


        public List<GridElement> GetNeighbourList()
        {
            List<GridElement> neighbourList = new List<GridElement>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Vector2Int neighbourLocation = gridLocation + new Vector2Int(i, j);
                    GridElement neighbourElement = grid.GetGridElement(neighbourLocation);

                    if (neighbourElement == null) continue;

                    neighbourList.Add(neighbourElement);
                }
            }

            return neighbourList;
        }
    }
}
