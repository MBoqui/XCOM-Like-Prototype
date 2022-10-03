using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GridSystem.Grid;
using GridSystem;
using GameStates;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    Grid grid;
    StateMachine stateMachine;

    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;

        stateMachine = new StateMachine();
    }


    void Start()
    {
        stateMachine.SetState(new GameSetup(stateMachine));
    }

    void Update()
    {
        if (stateMachine == null) return;

        stateMachine.Execute();
    }

    public void ExitState()
    {
        stateMachine.Exit();
    }

    public void NewGame()
    {
        grid = new Grid(GameSettings.Instance.gridSize);
        stateMachine.grid = grid;

        UnitManager.Instance.InitializeNewArmies(grid);
        TreeManager.Instance.InitializeRandomTrees(grid);
    }
}
