using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArmyCreator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TMP_InputField numberTanks;

    int playerIndex;


    public void SetPlayerIndex(int playerIndex)
    {
        this.playerIndex = playerIndex;
        title.text = "Player " + playerIndex;
    }


    public Army GetArmy()
    {
        Color color = GameSettings.Instance.GetPlayerColor(playerIndex);
        return new Army(playerIndex, color, int.Parse(numberTanks.text));
    }


    public void RemoveArmy()
    {
        GameSetupMenu.Instance.RemovePlayer(this);
        GameSetupMenu.Instance.RecalculateplayerIndex();
        Destroy(gameObject);
    }
}
