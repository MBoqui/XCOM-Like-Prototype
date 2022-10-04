using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;

public class Tank : Hittable
{
    //Tank parts
    [SerializeField] Transform tankHead;
    [SerializeField] Transform tankWeapon;
    [SerializeField] Transform weaponTip;
    [SerializeField] Transform bodyTarget;

    //aim info
    [SerializeField] int aimNumberLayers = 10;
    [SerializeField] int aimRotationPrecision = 10;
    [SerializeField] float aimDegreesError = 5;

    //weapon info
    [SerializeField] Vector2Int weaponDamage = new Vector2Int(20, 40);
    [SerializeField] int _weaponAPCost = 45;
    [SerializeField] ParticleSystem muzzleEffect;
    [SerializeField] GameObject hitEffect;
    public int weaponAPCost { get => _weaponAPCost; }

    //AP info
    [SerializeField] int maxAP = 100;
    public int currentAP { get; private set; }

    //script info
    public int playerIndex { get; private set; }
    public Vector2Int gridPosition { get => agent.gridPosition; }
    GridAgent agent;


    //Unity Messages
    new void Awake()
    {
        base.Awake();
        agent = GetComponent<GridAgent>();
        RefreshAP();
    }


    void OnDestroy()
    {
        UnitManager.Instance.RemoveTank(this);
    }


    //public Methods
    public void Initialize(int playerIndex, Color playerColor)
    {
        this.playerIndex = playerIndex;
        SetColor(playerColor);
    }


    public void SetMovePath(int pathCost, List<Vector2Int> path)
    {
        if (!TrySpendAP(pathCost)) return; //unit dont have enough AP
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
                (bool hit, RaycastHit raycastHit) = RaycastWithAimError(i * deviationStep, j * angleStep, LayerMask.GetMask("Tank"));

                if (!hit) continue; //didnt hit anything
                if (raycastHit.rigidbody == null) continue; //didnt hit anything destroyable

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
        if (!TrySpendAP(weaponAPCost)) return; //unit dont have enough AP

        muzzleEffect.Play();

        float deviation = Random.Range(0f, aimDegreesError);
        float angle = Random.Range(0f, 360f);

        (bool hit, RaycastHit raycastHit) = RaycastWithAimError(deviation, angle, ~0);

        if (!hit) return; //didnt hit anything

        StartCoroutine(PlayHitEffect(raycastHit.point)); //run hit effect

        if (raycastHit.rigidbody == null) return;

        Hittable hittable = raycastHit.rigidbody.GetComponent<Hittable>();

        if (hittable == null) return; //didnt hit anything destroyable

        int damageAmount = Random.Range(weaponDamage.x, weaponDamage.y + 1);
        hittable.TakeDamage(damageAmount);
    }
    

    public Vector3 GetTargetPoint()
    {
        return bodyTarget.position;
    }


    public void RefreshAP()
    {
        currentAP = maxAP;
    }


    public bool TrySpendAP(int amount)
    {
        if (currentAP < amount) return false;

        currentAP -= amount;
        return true;
    }


    public bool IsBusy()
    {
        return agent.IsBusy();
    }


    //private Methods
    Vector3 GetAimDirectionWithError(float deviation, float angle)
    {
        Vector3 aimVector = Vector3.forward;
        aimVector = Quaternion.AngleAxis(deviation, Vector3.up) * aimVector;
        aimVector = Quaternion.AngleAxis(angle, Vector3.forward) * aimVector;
        aimVector = weaponTip.rotation * aimVector;

        return aimVector;
    }


    (bool, RaycastHit) RaycastWithAimError(float deviation, float angle, int layerMask)
    {
        Vector3 rayDirection = GetAimDirectionWithError(deviation, angle);
        Ray ray = new Ray(weaponTip.position, rayDirection);

        bool hit = Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layerMask);

        return (hit, raycastHit);
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

    IEnumerator PlayHitEffect(Vector3 position)
    {
        GameObject gameObject = Instantiate(hitEffect, position, Quaternion.identity);

        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }
}
