using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;

public class Tank : MonoBehaviour
{
    [SerializeField] Transform tankHead;
    [SerializeField] Transform tankWeapon;
    [SerializeField] Transform weaponTip;
    [SerializeField] Transform bodyTarget;

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


    public float CalculateHitChance(Tank target)
    {
        Vector3 aimTarget = target.GetTargetPoint();
        AimAt(aimTarget);


        return 0;
    }


    public void Attack(Tank target)
    {
        Debug.Log("Attack");
    }


    public Vector3 GetTargetPoint()
    {
        return bodyTarget.position;
    }


    void AimAt(Vector3 target)
    {
        Vector3 headTurnDirection = new Vector3(target.x, 0, target.z);
        tankHead.LookAt(headTurnDirection);

        tankWeapon.LookAt(target);
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
