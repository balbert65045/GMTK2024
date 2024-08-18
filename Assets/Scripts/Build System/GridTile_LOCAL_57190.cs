using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{

    private Renderer _renderer;
    private readonly Color _highlightColor = new(0, 1, 0, 0.7f);
    private readonly Color _defaultColor = new(0, 0, 1, 0.5f);
    private readonly Color _cantPlaceColor = new(1, 0, 0, 0.9f);
    private Stack<GridTile> _currentHighlightedTiles = new Stack<GridTile>();
    private bool _canPlace = true;
    private int _x;
    private int _y;
    Block _blockHolding;
    

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
        _canPlace = true;

        GridVisualizer gridVisualizer = GetComponentInParent<GridVisualizer>();
        GridTile[,] grid = gridVisualizer.GetGrid();

        //Bens AWESOME changes
        if (_renderer != null) _renderer.material.color = GridSelectionManager.Instance.GetHighlightColor();
        Block selectedBlock = FindObjectOfType<GridSelectionManager>().GetCurrentPrefab();
        if (selectedBlock == null) return;
        List<Vector2> blockNeighbors = selectedBlock.GetNeighbors();
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        List<GridTile> previousNeighbors = new List<GridTile>();
        Block[] blockComponents = new Block[blocks.Length];

        for (int i = 0; i < blocks.Length; i++)
        {
            blockComponents[i] = blocks[i].GetComponent<Block>();
        }

        if (selectedBlock.GetBlockType() == BlockType.SQUARE)
        {
            foreach (Block block in blockComponents)
            {
                if (block == null) continue;
                List<Vector2> gridPositions = block.GetGridPositions();
                if (gridPositions == null) continue;
                foreach (Vector2 gridPosition in gridPositions)
                {
                    if (gridPosition == null) continue;
                    if (gridPosition == new Vector2(_x, _y))
                    {
                        _canPlace = false;
                        _renderer.material.color = _cantPlaceColor;
                        return;
                    }
                }
            }
        }

        foreach (Vector2 neighbor in blockNeighbors)
        {
            foreach (Block block in blockComponents)
            {
                if (block == null) continue;
                List<Vector2> gridPositions = block.GetGridPositions();
                if (gridPositions == null) continue;
                foreach (Vector2 gridPosition in gridPositions)
                {
                    if (gridPosition == null) continue;
                    if (gridPosition == new Vector2(_x + neighbor.x, _y + neighbor.y) || gridPosition == new Vector2(_x, _y))
                    {
                        foreach (GridTile tile in previousNeighbors)
                        {
                            tile.GetComponent<Renderer>().material.color = _cantPlaceColor;
                        }
                        _canPlace = false;
                        _renderer.material.color = _cantPlaceColor;
                        return;
                    }
                }
            }
            
            int x = _x + (int)neighbor.x;
            int y = _y + (int)neighbor.y;
            if (x < 0 || x >= gridVisualizer.GetWidth() || y < 0 || y >= gridVisualizer.GetHeight())
            {
                foreach(GridTile tile in previousNeighbors)
                {
                    tile.GetComponent<Renderer>().material.color = _cantPlaceColor;
                }
                _canPlace = false;
                _renderer.material.color = _cantPlaceColor;
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
                        tileRenderer.material.color = GridSelectionManager.Instance.GetHighlightColor();
                    }
                }
                _currentHighlightedTiles.Push(this);
                _currentHighlightedTiles.Push(tile);
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
        _currentHighlightedTiles.Clear();
        GridVisualizer gridVisualizer = GetComponentInParent<GridVisualizer>();
        GridTile[,] grid = gridVisualizer.GetGrid();

        //Bens stupid changes
        if (_renderer != null) _renderer.material.color = _defaultColor;
        Block selectedBlock = FindObjectOfType<GridSelectionManager>().GetCurrentPrefab();
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
        FindObjectOfType<GridSelectionManager>().SetTileOver(this);
        Block selectedBlock = FindObjectOfType<GridSelectionManager>().GetCurrentPrefab();
        if (selectedBlock == null) return;
        HighlightTiles();
    }

    void OnMouseExit()
    {
        if (FindObjectOfType<GridSelectionManager>().GetTileOver() == this)
        {
            FindObjectOfType<GridSelectionManager>().SetTileOver(null);
        }
        Block selectedBlock = FindObjectOfType<GridSelectionManager>().GetCurrentPrefab();
        if (selectedBlock == null) return;
        UnhighlightTiles();
    }

    public void SetColor(Color color)
    {
        _renderer.material.color = color;
    }


    public void SetGridPosition(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public Vector2 GetGridPosition()
    {
        return new Vector2(_x, _y);
    }

    public Block GetBlockHolding() { return _blockHolding; }

    public void SetBlockHolding(Block block) { _blockHolding = block; }

    public Stack<GridTile> GetCurrentHighlightedTiles() { return _currentHighlightedTiles; }

    public bool GetCanPlace() { return _canPlace; }
}
