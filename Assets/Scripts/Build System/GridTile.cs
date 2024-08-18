using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridTile : MonoBehaviour
{

    private Renderer _renderer;
    private readonly Color _highlightColor = new(0, 1, 0, 0.7f);
    private readonly Color _defaultColor = new(0, 0, 1, 0.5f);
    private int _x;
    private int _y;
    Block _blockHolding;
    public Block GetBlockHolding() { return _blockHolding; }
    public void SetBlockHolding(Block block)
    {
        _blockHolding = block;
    }

    void Start()
    {
        // set the layer to Grid
        gameObject.layer = LayerMask.NameToLayer("Grid");
        gameObject.tag = "GridTile";
        // store reference of the block

        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
        {
            _renderer.material.shader = Shader.Find("Transparent/Diffuse");
            _renderer.material.color = _defaultColor;
        }
    }


    public void HighlightTiles()
    {
        GridVisualizer gridVisualizer = GetComponentInParent<GridVisualizer>();
        GridTile[,] grid = gridVisualizer.GetGrid();

        //Bens stupid changes
        if (_renderer != null) _renderer.material.color = _highlightColor;
        Block selectedBlock = FindObjectOfType<GridSelectionManager>().prefabBlockPlacing;
        if (selectedBlock == null) { return; }
        List<Vector2> blockNeighbors = selectedBlock.GetNeighbors();
        List<GridTile> previousNeighbors = new List<GridTile>();
        foreach (Vector2 neighbor in blockNeighbors)
        {
            int x = _x + (int)neighbor.x;
            int y = _y + (int)neighbor.y;
            if (x < 0 || x >= gridVisualizer.GetWidth() || y < 0 || y >= gridVisualizer.GetHeight())
            {
                foreach(GridTile tile in previousNeighbors)
                {
                    tile.GetComponent<Renderer>().material.color = Color.red;
                }
                _renderer.material.color = Color.red;
                return;
            }
            else if (x >= 0 && x < gridVisualizer.GetWidth() && y >= 0 && y < gridVisualizer.GetHeight())
            {
                GridTile tile = grid[x, y];
                previousNeighbors.Add(tile);
                if (tile != null)
                {
                    Renderer tileRenderer = tile.GetComponent<Renderer>();
                    if (tileRenderer != null)
                    {
                        tileRenderer.material.color = _highlightColor;
                    }
                }

                
            }
            
        }
    }

    public List<GridTile> GetTileNeighbors(List<Vector2> blockNeighbors)
    {
        GridVisualizer gridVisualizer = GetComponentInParent<GridVisualizer>();
        List<GridTile> tileNeighbors = new List<GridTile>();
        foreach (Vector2 neighbor in blockNeighbors)
        {
            int x = _x + (int)neighbor.x;
            int y = _y + (int)neighbor.y;
            tileNeighbors.Add(gridVisualizer.GetGrid()[x, y]);
        }
        return tileNeighbors;
    }

    void UnhighlightTiles()
    {
        GridVisualizer gridVisualizer = GetComponentInParent<GridVisualizer>();
        GridTile[,] grid = gridVisualizer.GetGrid();

        //Bens stupid changes
        if (_renderer != null) _renderer.material.color = _defaultColor;
        Block selectedBlock = FindObjectOfType<GridSelectionManager>().prefabBlockPlacing;
        List<Vector2> blockNeighbors = selectedBlock.GetNeighbors();
        foreach (Vector2 neighbor in blockNeighbors)
        {
            int x = _x + (int)neighbor.x;
            int y = _y + (int)neighbor.y;
            if (x >= 0 && x < gridVisualizer.GetWidth() && y >= 0 && y < gridVisualizer.GetHeight())
            {
                GridTile tile = grid[x, y];
                if (tile != null)
                {
                    Renderer tileRenderer = tile.GetComponent<Renderer>();
                    if (tileRenderer != null)
                    {
                        tileRenderer.material.color = _defaultColor;
                    }
                }
            }
        }
    }

    void OnMouseEnter()
    {
        FindObjectOfType<GridSelectionManager>().tileOver = this;
        Block selectedBlock = FindObjectOfType<GridSelectionManager>().prefabBlockPlacing;
        if (selectedBlock == null) { return; }
        HighlightTiles();
    }

    void OnMouseExit()
    {
        if (FindObjectOfType<GridSelectionManager>().tileOver == this)
        {
            FindObjectOfType<GridSelectionManager>().tileOver = null;
        }
        Block selectedBlock = FindObjectOfType<GridSelectionManager>().prefabBlockPlacing;
        if (selectedBlock == null) { return; }
        UnhighlightTiles();
    }


    public void SetGridPosition(int x, int y)
    {
        _x = x;
        _y = y;
    }
}
