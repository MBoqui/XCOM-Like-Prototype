using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class GridElement : MonoBehaviour
    {
        Vector2Int gridLocation;

        //TerrainType ...
        GridObject gridObject;

        public GridElement(Vector2Int gridLocation)
        {
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

        public void RemoveGridObject()
        {
            gridObject = null;
        }

        public void DestroyGridObject()
        {
            gridObject.DestroySelf();
        }
    }
}
