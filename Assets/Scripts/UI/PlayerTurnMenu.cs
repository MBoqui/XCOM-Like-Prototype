using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStates;
using TMPro;

public class PlayerTurnMenu : MonoBehaviour
{
    public static PlayerTurnMenu Instance;

    [SerializeField] TextMeshProUGUI instructions;
    [SerializeField] TextMeshProUGUI stats;

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

    public void SetInfo(Tank selectedUnit, int pathCost, float hitChance = -1)
    {
        UpdateStats (selectedUnit);

        UpdateInstructions(selectedUnit, pathCost, hitChance);
    }

    void UpdateStats (Tank selectedUnit)
    {
        if (selectedUnit == null)
        {
            stats.text = "";
            return;
        }

        stats.text = "Health: " + selectedUnit.currentLife +
            "\nAP: " + selectedUnit.currentAP;
    }

    void UpdateInstructions(Tank selectedUnit, int pathCost = 0, float hitChance = -1)
    {
        if (selectedUnit == null)
        {
            instructions.text = "Click on one of your units to select it.";
            return;
        }

        string text = "Click on one of your units to select it" +
            "\nClick on an enemy unit to attack (" + selectedUnit.weaponAPCost + " AP)";

        if (hitChance >= 0)
        {
            text += " / chance to hit: " + Mathf.Round(hitChance * 100) + "%";
        }

        text += "\nClick on an empty space to move";

        if (pathCost != 0)
        {
            text += "(" + pathCost + " AP)";
        }

        instructions.text = text;
    }
}
