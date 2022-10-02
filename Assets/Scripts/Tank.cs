using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;

public class Tank : MonoBehaviour
{
    [SerializeField] GameObject tankHead;
    [SerializeField] GameObject tankWeapon;

    public int playerIndex { get; private set; }
    public Vector2Int gridPosition { get => agent.gridPosition; }
    GridAgent agent;


    void Awake()
    {
        agent = GetComponent<GridAgent>();
    }


    public void Initialize(int playerIndex, Color playerColor)
    {
        this.playerIndex = playerIndex;
        SetColor(playerColor);
    }


    public void SetMovePath(List<Vector2Int> path)
    {
        agent.SetMovePath(path);
    }


    public void Attack(Tank target)
    {
        Debug.Log("Attack");
    }


    void SetColor(Color newColor)
    {
        gameObject.GetComponent<Renderer>().material.color = newColor;
        tankHead.GetComponent<Renderer>().material.color = newColor;
        tankWeapon.GetComponent<Renderer>().material.color = newColor;
    }


    void OnDestroy()
    {
        UnitManager.Instance.RemoveTank(this);
    }
}
