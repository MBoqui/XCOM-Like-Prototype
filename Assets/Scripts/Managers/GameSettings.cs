using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    public Vector2Int gridSize { get; private set; }
    public float treeDensity { get; private set; }
    public int numberPlayers { get; private set; }

    [SerializeField] Color[] playerColors;


    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    public void WriteValues(Vector2Int gridSize, float treeDensity, int numberPlayers)
    {
        this.gridSize = gridSize;
        this.treeDensity = treeDensity;
        this.numberPlayers = numberPlayers;
    }

    public Color GetColor(int colorIndex)
    {
        return playerColors[colorIndex];
    }
}
