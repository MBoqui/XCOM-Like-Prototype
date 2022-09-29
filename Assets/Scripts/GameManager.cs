using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GridSystem.Grid;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Vector2Int gridSize = new Vector2Int(128, 128);
    Grid grid;

    void Awake()
    {
        grid = new Grid(gridSize);
    }

    void Update()
    {
        Vector3 worldMousePosition = Boqui.Utils.GetMouseWorldPosition(LayerMask.GetMask("Ground"));
        if (Input.GetMouseButtonDown(0))
        {
            grid.TryAddObject(prefab, worldMousePosition);
        }
    }
}
