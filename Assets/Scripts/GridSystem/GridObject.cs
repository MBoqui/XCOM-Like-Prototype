using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class GridObject : MonoBehaviour
    {
        public Grid grid { get; private set; }
        public Vector2Int gridPosition { get => element.gridPosition; }
        public bool isBlocker { get; private set; }

        GridElement element;


        public static GridObject Create(Grid grid, GameObject prefab, GridElement targetElement, bool isBlocker)
        {
            //check if object can be built in grid
            Vector3 worldPosition = (Vector3)grid.GetWorldPosition(targetElement.gridPosition);

            //instantiate prefab
            GameObject gridObjectGameObject = Instantiate(prefab, worldPosition, Quaternion.identity);

            //setup component
            GridObject gridObject = gridObjectGameObject.AddComponent<GridObject>();
            gridObject.grid = grid;
            gridObject.isBlocker = isBlocker;
            gridObject.SetGridElement(targetElement);

            return gridObject;
        }


        public void SetGridElement(GridElement newElement)
        {
            if (element != null) {
                element.ClearGridObject();
            }

            element = newElement;
        }


        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        void OnDestroy()
        {
            element.ClearGridObject();
        }
    }
}
