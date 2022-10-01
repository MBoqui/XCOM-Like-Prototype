using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class GridObject : MonoBehaviour
    {
        public Grid grid { get; private set; }
        Vector2Int gridLocation;


        public static GridObject Create(Grid grid, GameObject prefab, Vector2Int gridPosition)
        {
            //check if object can be built in grid
            Vector3? worldPosition = grid.GetWorldPosition(gridPosition);
            if (worldPosition == null)
            {
                Debug.LogWarning("tried adding object outside of grid");
                return null;
            }

            //instantiate prefab
            GameObject gridObjectGameObject = Instantiate(prefab, (Vector3)worldPosition, Quaternion.identity);

            //setup component
            GridObject gridObject = gridObjectGameObject.AddComponent<GridObject>();
            gridObject.grid = grid;
            gridObject.gridLocation = gridPosition;

            return gridObject;
        }


        public void SetGridLocation(Vector2Int newGridLocation)
        {
            if (!grid.IsInGrid(newGridLocation))
            {
                Debug.LogError("tried setting GridObject outside of grid");
                return;
            }
            gridLocation = newGridLocation;
        }


        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
