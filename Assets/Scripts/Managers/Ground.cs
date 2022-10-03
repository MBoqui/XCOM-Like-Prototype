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
    Vector2 perlinOffset;

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
        perlinOffset = new Vector2(Random.Range(-10000, 10000), Random.Range(-10000, 10000));

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
        for (int i = 1; i < terrains.Length; i++)
        {
            if (TrySetTerrainType(i, gridPosition)) return;
        }

        grid.GetGridElement(gridPosition).SetTerrainType(terrains[0]);
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


    bool TrySetTerrainType(int terrainIndex, Vector2Int gridPosition)
    {
        Vector2 scale = terrains[terrainIndex].generationScale;
        float perlinX = (float)gridPosition.x * terrainIndex * scale.x + perlinOffset.x;
        float perlinY = (float)gridPosition.y * terrainIndex * scale.y + perlinOffset.y;

        float sample = Mathf.PerlinNoise(perlinY, perlinX);

        if (sample <= terrains[terrainIndex].generationCutoff)
        {
            grid.GetGridElement(gridPosition).SetTerrainType(terrains[terrainIndex]);
            return true;
        }

        return false;
    }
}
