using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainType", menuName = "ScriptableObjects/TerrainType")]
public class TerrainType : ScriptableObject
{
    new public string name;

    public Color color1;
    public Color color2;
    public bool isBlocked;
    public int pathCostIncrement;
    public float generationCutoff = 0.5f;
    public Vector2 generationScale = new Vector2(20,20);
}
