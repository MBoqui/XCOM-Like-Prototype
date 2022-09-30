using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GridSystem.Grid;
using GridSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject tankPrefab;
    [SerializeField] Vector2Int gridSize = new Vector2Int(128, 128);
    Grid grid;
    Tank tank;
    PathFinder pathFinder;

    void Awake()
    {
        grid = new Grid(gridSize);
        pathFinder = new PathFinder(grid);
    }

    void Start()
    {
        tank = grid.TryAddObject(tankPrefab, Vector2Int.zero).GetComponent<Tank>();
    }

    void Update()
    {
        Vector3 worldMousePosition = Boqui.Utils.GetMouseWorldPosition(LayerMask.GetMask("Ground"));

        if (Input.GetMouseButtonDown(0))
        {
            grid.TryAddObject(prefab, worldMousePosition, true);
        }

        if (Input.GetMouseButtonDown(1))
        {
            grid.TryDestroyObject(worldMousePosition);
        }

        if (Input.GetMouseButtonDown(2))
        {
            Vector2Int? targetlocation = grid.GetGridPosition(worldMousePosition);

            if (targetlocation == null) return;

            List<Vector2Int> path = pathFinder.FindPath(tank.GetGridPosition(), (Vector2Int)targetlocation);

            if (path != null)
            {
                tank.SetMovePath(path);

                for(int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x + 0.5f, 0, path[i].y + 0.5f), new Vector3(path[i + 1].x + 0.5f, 0 , path[i + 1].y + 0.5f), Color.red, 5f);
                }
            }
        }
    }
}
