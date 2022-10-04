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

    new Transform transform;

    public List<Tank> allTanks = new List<Tank> ();
    Army[] armies;


    //unity Messages
    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;

        transform = GetComponent<Transform>();
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
        CheckForEliminated(tank.playerIndex);
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


    public Vector2Int GetPlayerFristUnitPosition(int playerIndex)
    {
        foreach (Tank tank in allTanks)
        {
            if (tank.playerIndex == playerIndex) return tank.gridPosition;
        }

        return Vector2Int.zero;
    }


    public bool GetArmyEliminated(int playerindex)
    {
        return armies[playerindex - 1].isEliminated;
    }


    //private Methods
    bool TryAddTank(Army army, Vector2Int gridPosition)
    {
        GridObject newUnit = grid.TryAddObject(tankPrefab, gridPosition, transform);

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
        int aliveCount = 0;
        int winnerIndex = 0;
        foreach(Army army in armies)
        {
            if (!army.isEliminated)
            {
                winnerIndex = army.playerIndex;
                aliveCount++;
                if (aliveCount > 1) return;
            }
        }

        //if only one player remain, end game
        GameManager.Instance.DeclareWinner(winnerIndex);
    }


    void CheckForEliminated(int playerIndex)
    {
        foreach (Tank tank in allTanks)
        {
            if (playerIndex == tank.playerIndex) return;
        }

        armies[playerIndex - 1].SetEliminated();
    }
}
