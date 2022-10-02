using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GridSystem.Grid;
using GridSystem;
using GameStates;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject tankPrefab;
    [SerializeField] Vector2Int gridSize = new Vector2Int(128, 128);

    Grid grid;
    StateMachine stateMachine;

    void Awake()
    {
        grid = new Grid(gridSize);
        stateMachine = new StateMachine(grid, 1, tankPrefab);
    }


    void Start()
    {
        TreeManager.Instance.Initiallize(grid);
        stateMachine.SetState(new GameSetup(stateMachine));
    }

    void Update()
    {
        stateMachine.Execute();
    }
}
