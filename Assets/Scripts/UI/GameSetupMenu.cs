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

    [SerializeField] Transform content;
    [SerializeField] GameObject armyCreatorPrefab;

    List<ArmyCreator> creators = new List<ArmyCreator>();


    //Unity Messages
    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;

        AddPlayer();
        AddPlayer();

        Disable();
    }


    //public Methods
    public void StartMatch()
    {
        WriteToSettings();
        GameManager.Instance.ExitState();
    }


    public void AddPlayer()
    {
        if (creators.Count >= 8) return;

        GameObject gameObject = Instantiate(armyCreatorPrefab, content);

        ArmyCreator creator = gameObject.GetComponent<ArmyCreator>();

        creators.Add(creator);

        creator.SetPlayerIndex(creators.Count);
    }


    public void RecalculateplayerIndex()
    {
        for (int i = 0; i < creators.Count; i++)
        {
            creators[i].SetPlayerIndex(i + 1);
        }
    }


    public void RemovePlayer(ArmyCreator creator)
    {
        creators.Remove(creator);
    }


    public void Disable()
    {
        gameObject.SetActive(false);
    }


    public void Enable()
    {
        gameObject.SetActive(true);
    }


    public int GetNumberPlayers()
    {
        return creators.Count;
    }


    //private Methods
    void WriteToSettings()
    {
        Vector2Int gridSize = new Vector2Int(int.Parse(gridSizeX.text), int.Parse(gridSizeY.text));
        Army[] armies = GetArmies();
        UnitManager.Instance.SetArmies(armies);
        GameSettings.Instance.WriteValues(gridSize, treeDensity.value, armies.Length);
    }


    Army[] GetArmies()
    {
        List<Army> armies = new List<Army>();

        for(int i = 0; i < creators.Count; i++)
        {
            armies.Add(creators[i].GetArmy());
        }

        return armies.ToArray();
    }
}
