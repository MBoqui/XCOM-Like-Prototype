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


        //Unity Messages
        void OnDestroy()
        {
            element.ClearGridObject();
        }


        //public Methods
        public static GridObject Create(Grid grid, GameObject prefab, GridElement targetElement, Transform parent, bool isBlocker, bool randomizeTransform = false)
        {
            //get position and rotation to instantiate
            Vector3 worldPosition = (Vector3)grid.GetWorldPosition(targetElement.gridPosition);
            Quaternion rotation = Quaternion.identity;

            if (randomizeTransform)
            {
                worldPosition += new Vector3(Random.value / 4, 0, Random.value / 4);
                rotation *= Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
            }

            //instantiate prefab
            GameObject gridObjectGameObject = Instantiate(prefab, worldPosition, rotation, parent);

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
    }
}
