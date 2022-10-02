using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;

public class Tank : MonoBehaviour
{
    public int playerIndex { get; private set; }
    public Vector2Int gridPosition { get => agent.gridPosition; }
    GridAgent agent;


    void Awake()
    {
        agent = GetComponent<GridAgent>();
    }


    public void Initialize(int playerIndex)
    {
        this.playerIndex = playerIndex;
    }


    public void SetMovePath(List<Vector2Int> path)
    {
        agent.SetMovePath(path);
    }


    public void Attack(Tank target)
    {
        Debug.Log("Attack");
    }


    void OnDestroy()
    {
        UnitManager.Instance.RemoveTank(this);
    }
}
