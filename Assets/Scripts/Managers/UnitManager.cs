using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GridSystem.Grid;
using GridSystem;


public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    [SerializeField] GameObject tankPrefab;
    Grid grid;

    public List<Tank> allTanks = new List<Tank> ();
    Army[] armies;


    //unity Messages
    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }


    //public Methods
    public void InitializeNewArmies(Grid grid)
    {
        this.grid = grid;

        ClearTanks();

        for (int i = 0; i < armies.Length; i++)
        {
            Vector2Int centerSpawn = grid.GetRandomPosition();

            for (int j = 0; j < armies[i].numberTanks; j++)
            {
                Vector2Int position = grid.FindAvailablePositionAround(centerSpawn);
                TryAddTank(armies[i], position);
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


    public void SetArmies(Army[] armies)
    {
        this.armies = armies;
    }


    //private Methods
    bool TryAddTank(Army army, Vector2Int gridPosition)
    {
        GridObject newUnit = grid.TryAddObject(tankPrefab, gridPosition);

        if (newUnit != null)
        {
            Tank newTank = newUnit.GetComponent<Tank>();
            newTank.Initialize(army.playerIndex, army.color);

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
