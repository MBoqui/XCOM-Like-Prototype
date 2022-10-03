using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    [SerializeField] int maxLife = 100;

    public int currentLife { get; private set; }

    protected void Awake()
    {
        currentLife = maxLife;
    }

    public void TakeDamage(int amount)
    {
        currentLife -= amount;

        if (currentLife <= 0)
        {
            StartCoroutine("Die");
        }
    }

    IEnumerator Die()
    {
        while (transform.localScale.x > 0.05f)
        {
            transform.localScale *= 0.8f;
            yield return null;
        }
        Destroy(gameObject);
    }
}
