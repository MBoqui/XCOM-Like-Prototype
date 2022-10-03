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
    [SerializeField] Color[] playerColors;

    public List<Tank> allTanks = new List<Tank> ();


    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }


    public void InitializeNewArmies(Grid grid)
    {
        this.grid = grid;

        ClearTanks();

        for (int i = 0; i < GameSettings.Instance.numberPlayers; i++)
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
        CheckForWinner();
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


    void ClearTanks()
    {
        foreach (Tank tank in allTanks)
        {
            Destroy(tank.gameObject);
        }
    }


    void CheckForWinner()
    {
        int lastPlayerIndex = allTanks[0].playerIndex;
        foreach(Tank tank in allTanks)
        {
            if (lastPlayerIndex != tank.playerIndex) return;
        }

        //if all remaining tanks belong to the same player, end game
        GameManager.Instance.DeclareWinner(lastPlayerIndex);
    }
}
