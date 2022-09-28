using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GridSystem.Grid;

public class GameManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize = new Vector2Int(128, 128);
    Grid grid;

    void Awake()
    {
        grid = new Grid(gridSize);
    }

    void Update()
    {
        Vector3 worldPos = Boqui.Utils.GetMouseWorldPosition(LayerMask.GetMask("Ground"));
        Debug.Log(grid.GetXY(worldPos));
    }
}
