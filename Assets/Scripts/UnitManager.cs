using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GridSystem.Grid;
using GridSystem;


public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    [SerializeField] GameObject tankPrefab;
    [SerializeField] int unitsPerPlayer = 6;
    Grid grid;
    int numberPlayers;
    [SerializeField] Color[] playerColors;

    public List<Tank> allTanks = new List<Tank> ();


    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }


    public void Initiallize(Grid grid, int numberPlayers)
    {
        this.grid = grid;
        this.numberPlayers = numberPlayers;
    }


    public void GenerateArmies()
    {
        for (int i = 0; i < numberPlayers; i++)
        {
            for (int j = 0; j < unitsPerPlayer; j++)
            {
                TryAddTank(i + 1, new Vector2Int(i, j));
            }
        }
    }

    public void RemoveTank(Tank tank)
    {
        allTanks.Remove(tank);
    }


    public void RefreshUnits(int playerIndex)
    {
        foreach(Tank tank in allTanks)
        {
            if (tank.playerIndex != playerIndex) continue;

            tank.RefreshAP();
        }
    }

    bool TryAddTank(int playerIndex, Vector2Int gridPosition)
    {
        GridObject newUnit = grid.TryAddObject(tankPrefab, gridPosition);

        if (newUnit != null)
        {
            Tank newTank = newUnit.GetComponent<Tank>();
            newTank.Initialize(playerIndex, playerColors[playerIndex - 1]);

            allTanks.Add(newTank);
        }

        return newUnit;
    }
}
