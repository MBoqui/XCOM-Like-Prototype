using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army
{
    public int playerIndex { get; private set; }
    public Color color { get; private set; }
    public int numberTanks { get; private set; }

    public bool isEliminated { get; private set; }


    //public Methods
    public Army (int playerIndex, Color color, int numberTanks)
    {
        this.playerIndex = playerIndex;
        this.color = color;
        this.numberTanks = numberTanks;
    }


    public void SetEliminated()
    {
        isEliminated = true;
    }
}
