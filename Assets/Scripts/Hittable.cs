using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    [SerializeField] int maxLife = 100;

    public int currentLife { get; private set; }


    //Unity Messages
    protected void Awake()
    {
        currentLife = maxLife;
    }


    //public Methods
    public void TakeDamage(int amount)
    {
        currentLife -= amount;

        if (currentLife <= 0)
        {
            StartCoroutine("Die");
        }
    }


    //Coroutines
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
