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

    [SerializeField] int aimNumberLayers = 10;
    [SerializeField] int aimRotationPrecision = 10;
    [SerializeField] float aimDegreesError = 5;

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

        float deviationStep = aimDegreesError / aimNumberLayers;
        float angleStep = 360f / aimRotationPrecision;

        int hits = 0;

        for (int i = 1; i <= aimNumberLayers; i++)
        {
            for (int j = 0; j < aimRotationPrecision; j++)
            {
                Vector3 rayDirection = GetAimDirectionWithError(i * deviationStep, j * angleStep);
                Ray ray = new Ray(weaponTip.position, rayDirection);

                Debug.DrawRay(weaponTip.position, rayDirection * 10);

                if (!Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, LayerMask.GetMask("Tank"))) continue; //didnt hit anything

                Tank hitTank = raycastHit.rigidbody.GetComponent<Tank>();

                if (hitTank == null) continue; //didnt hit a tank
                if (hitTank.playerIndex == playerIndex) continue; //hit your own tank
                
                hits++;
            }
        }

        return (float)hits / (aimNumberLayers * aimRotationPrecision);
    }


    public void Attack(Tank target)
    {
        float deviation = Random.Range(0f, aimDegreesError);
        float angle = Random.Range(0f, 360f);

        Debug.Log("Attack");
    }


    Vector3 GetAimDirectionWithError(float deviation, float angle)
    {
        Vector3 aimVector = Vector3.forward;
        aimVector = Quaternion.AngleAxis(deviation, Vector3.up) * aimVector;
        aimVector = Quaternion.AngleAxis(angle, Vector3.forward) * aimVector;
        aimVector = weaponTip.rotation * aimVector;

        return aimVector;
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
