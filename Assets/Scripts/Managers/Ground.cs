using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = GridSystem.Grid;
using GridSystem;

public class Ground : MonoBehaviour
{
    public static Ground Instance { get; private set; }

    new Transform transform;
    new Renderer renderer;
    Grid grid;

    [SerializeField] TerrainType[] terrains;


    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;

        transform = GetComponent<Transform>();
        renderer = GetComponent<Renderer>();
    }


    public void InitializeNewMap(Grid grid)
    {
        this.grid = grid;
        SetupGround(grid.gridSize);

        for (int i = 0; i < grid.gridSize.x; i++)
        {
            for (int j = 0; j < grid.gridSize.y; j++)
            {
                 SetTerrainType(new Vector2Int(i, j));
            }
        }

        renderer.material.mainTexture = GenerateTexture();
    }


    void SetTerrainType(Vector2Int gridPosition)
    {
        grid.GetGridElement(gridPosition).SetTerrainType(terrains[0]);

        /*float chance = Random.value;

        if (chance <= density)
        {
            TryAddTree(new Vector2Int(i, j));
        }*/
    }

    void SetupGround(Vector2Int mapSize)
    {
        transform.position = new Vector3(mapSize.x / 2f, 0, mapSize.y / 2f);
        transform.localScale = new Vector3(mapSize.x / 10f, 1, mapSize.y / 10f);
    }


    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(grid.gridSize.x, grid.gridSize.y);

        for (int i = 0; i < grid.gridSize.x; i++)
        {
            for (int j = 0; j < grid.gridSize.y; j++)
            {
                bool primaryColor = (i + j) % 2 == 0;
                Color color = grid.GetGridElement(new Vector2Int(i, j)).GetTerrainColor(primaryColor);
                texture.SetPixel(i, j, color);
            }
        }

        texture.Apply();

        return texture;
    }
}
