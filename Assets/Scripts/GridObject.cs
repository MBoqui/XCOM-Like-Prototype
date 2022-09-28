using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class GridObject : MonoBehaviour
    {
        private Vector2Int gridLocation;

        public static GridObject Create(Grid grid, GameObject prefab, Vector3 worldPosition)
        {
            //instantiate prefab
            GameObject gridObjectGameObject = Instantiate(prefab, worldPosition, Quaternion.identity);

            //add this component and return
            GridObject gridObject = gridObjectGameObject.AddComponent<GridObject>();

            return gridObject;
        }
    }
}
