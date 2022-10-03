using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSetupMenu : MonoBehaviour
{
    public static GameSetupMenu Instance;

    [SerializeField] TMP_InputField gridSizeX;
    [SerializeField] TMP_InputField gridSizeY;
    [SerializeField] Slider treeDensity;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;

        Disable();
    }

    public void StartMatch()
    {
        WriteToSettings();
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


    void WriteToSettings()
    {
        Vector2Int gridSize = new Vector2Int(int.Parse(gridSizeX.text), int.Parse(gridSizeY.text));
        GameSettings.Instance.WriteValues(gridSize, treeDensity.value, 2);
    }
}
