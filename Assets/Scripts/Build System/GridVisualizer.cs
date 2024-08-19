using System;
using System.Collections;
using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    [SerializeField] private int _gridWidth = 16;
    [SerializeField] private int _gridHeight = 9;
    [SerializeField] private float _tileSize = 1.0f;
    [SerializeField] private float _gridScale = 0.98f;
    private GridTile[,] _grid;

    public GameObject GridObjectParent;

    private void Start()
    {
        _grid = new GridTile[_gridWidth, _gridHeight];
        CreateGrid();

        StartCoroutine(CheckForStaticBlocks());
    }

    public void HideGrid()
    {
        Renderer[] tiles = GetComponentsInChildren<Renderer>();
        foreach(Renderer tile in tiles)
        {
            tile.enabled = false;
        }
    }

    public void ShowGrid()
    {
        Renderer[] tiles = GetComponentsInChildren<Renderer>();
        foreach (Renderer tile in tiles)
        {
            tile.enabled = true;
        }
    }

    private void CreateGrid()
    {
        float gridWidth = _gridWidth * _tileSize;
        float gridHeight = _gridHeight * _tileSize;

        float startX = -(gridWidth / 2) + (_tileSize / 2);
        float startY = -(gridHeight / 2) + (_tileSize / 2);

        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {

                GameObject gridTile= GameObject.CreatePrimitive(PrimitiveType.Cube);
                gridTile.transform.parent = transform;
                gridTile.transform.localPosition = new Vector3(startX + x * _tileSize, startY + y * _tileSize, 0);

                // attach script to grid tile
                _grid[x, y] = gridTile.AddComponent<GridTile>();

                _grid[x, y].SetGridPosition(x, y);

                // set the scale
                gridTile.transform.localScale = new Vector3(_tileSize * _gridScale, _tileSize * _gridScale, 1);
            }
        }
    }

    private IEnumerator CheckForStaticBlocks()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject[] Block = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in Block)
        {
            if (block.GetComponent<Block>().IsBlockStatic())
            {
                block.GetComponent<Block>().SetStaticPosition();
            }
        }
    }

    public GridTile[,] GetGrid()
    {
        return _grid;
    }

    public int GetWidth()
    {
        return _gridWidth;
    }

    public int GetHeight()
    {
        return _gridHeight;
    }

    public Vector2 TranslateToGridPosition(Vector3 worldPosition)
    {
        float x = worldPosition.x + (_gridWidth * _tileSize / 2);
        float y = worldPosition.y + (_gridHeight * _tileSize / 2);

        x = x / _tileSize;
        y = y / _tileSize;

        return new Vector2((int)x, (int)y);
    }
}