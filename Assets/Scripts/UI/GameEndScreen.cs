using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameEndScreen : MonoBehaviour
{
    public static GameEndScreen Instance;

    [SerializeField] TextMeshProUGUI winnerText;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;

        Disable();
    }

    public void NewGame()
    {
        GameManager.Instance.ExitState();
    }


    public void CloseGame()
    {
        Application.Quit();
    }


    public void Disable()
    {
        gameObject.SetActive(false);
    }


    public void Enable(int playerIndex)
    {
        gameObject.SetActive(true);

        winnerText.text = "Player " + playerIndex + " is the winner!";
    }
}
