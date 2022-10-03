using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStates;

public class PlayerTurnMenu : MonoBehaviour
{
    public static PlayerTurnMenu Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;

        Disable();
    }

    public void EndTurn()
    {
        GameManager.Instance.ExitState();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }
}
