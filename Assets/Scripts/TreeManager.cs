using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GridSystem.Grid;
using GridSystem;


public class TreeManager : MonoBehaviour
{
    public static TreeManager Instance { get; private set; }

    [SerializeField] GameObject prefab;
    [SerializeField, Range(0, 1)] float treeDensity = 0.2f;
    Grid grid;

    List<GridObject> allTrees = new List<GridObject> ();


    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    public void Initiallize(Grid grid)
    {
        this.grid = grid;
    }


    public GridObject TryAddTree(Vector3 worldPosition)
    {
        GridObject newTree = grid.TryAddObject(prefab, worldPosition, true);

        if (newTree != null)
        {
            allTrees.Add(newTree);
        }

        return newTree;
    }


    public GridObject TryAddTree(Vector2Int gridPosition)
    {
        GridObject newTree = grid.TryAddObject(prefab, gridPosition, true);

        if (newTree != null)
        {
            allTrees.Add(newTree);
        }

        return newTree;
    }


    public void RemoveTree (Tree tree)
    {
        allTrees.Remove(tree.GetComponent<GridObject>());
    }


    public void GenerateRandomTrees()
    {
        for (int i = 0; i < grid.gridSize.x; i++)
        {
            for (int j = 0; j < grid.gridSize.y; j++)
            {
                float chance = Random.value;

                if (chance <= treeDensity)
                {
                    TryAddTree(new Vector2Int(i, j));
                }
            }
        }
    }
}
