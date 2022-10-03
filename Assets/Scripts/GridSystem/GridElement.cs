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
        public Vector2Int gridPosition { get; private set; }

        TerrainType terrainType;
        GridObject gridObject;

        //Pathfinding
        public int gCost;
        public int hCost;
        public int fCost;
        public GridElement parentElement;
        public GridAgent agent;


        public GridElement(Grid grid, Vector2Int gridPosition)
        {
            this.grid = grid;
            this.gridPosition = gridPosition;
        }


        public bool IsFree()
        {
            return gridObject == null;
        }


        public bool IsBlocked()
        {
            if (terrainType.isBlocked) return true;
            if (gridObject != null)
            {
                if (gridObject.isBlocker) return true;
            }

            return false;
        }


        public int GetTerrainCost()
        {
            return terrainType.pathCostIncrement;
        }


        public void SetGridObject(GridObject newGridObject)
        {
            newGridObject.SetGridElement(this);
            gridObject = newGridObject;
            agent = gridObject.GetComponent<GridAgent>();
        }


        public GridAgent GetGridAgent()
        {
            return agent;
        }


        public void ClearGridObject()
        {
            gridObject = null;
            agent = null;
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
                    Vector2Int neighbourLocation = gridPosition + new Vector2Int(i, j);
                    GridElement neighbourElement = grid.GetGridElement(neighbourLocation);

                    if (neighbourElement == null) continue;

                    neighbourList.Add(neighbourElement);
                }
            }

            return neighbourList;
        }


        public void SetTerrainType(TerrainType terrainType)
        {
            this.terrainType = terrainType;
        }


        public Color GetTerrainColor(bool primary = true)
        {
            if (primary)
            {
                return terrainType.color1;
            } else {
                return terrainType.color2;
            }
        }
    }
}
