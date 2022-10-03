using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GridSystem.Grid;
using GridSystem;
using GameStates;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Vector2Int gridSize = new Vector2Int(128, 128);
    [SerializeField] int numberPlayers = 1;

    Grid grid;
    StateMachine stateMachine;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;

        grid = new Grid(gridSize);
        stateMachine = new StateMachine(grid, numberPlayers);
    }


    void Start()
    {
        TreeManager.Instance.Initiallize(grid);
        UnitManager.Instance.Initiallize(grid, numberPlayers);
        stateMachine.SetState(new GameSetup(stateMachine));
    }

    void Update()
    {
        stateMachine.Execute();
    }

    public void ExitState()
    {
        stateMachine.Exit();
    }
}
