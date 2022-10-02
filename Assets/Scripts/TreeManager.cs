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
}
