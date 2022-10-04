using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GridSystem.Grid;
using GridSystem;


public class TreeManager : MonoBehaviour
{
    public static TreeManager Instance { get; private set; }

    [SerializeField] GameObject prefab;
    Grid grid;

    List<GridObject> allTrees = new List<GridObject> ();


    //Unity Messages
    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }


    //public Methods
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


    public void InitializeRandomTrees(Grid grid)
    {
        this.grid = grid;

        ClearTrees();

        float density = GameSettings.Instance.treeDensity;

        for (int i = 0; i < grid.gridSize.x; i++)
        {
            for (int j = 0; j < grid.gridSize.y; j++)
            {
                float chance = Random.value;

                if (chance <= density)
                {
                    TryAddTree(new Vector2Int(i, j));
                }
            }
        }
    }


    //private Methods
    void ClearTrees()
    {
        foreach (GridObject tree in allTrees)
        {
            tree.DestroySelf();
        }
    }
}
