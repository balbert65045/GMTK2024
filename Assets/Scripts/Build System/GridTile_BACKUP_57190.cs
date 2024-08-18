using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{

    private Renderer _renderer;
    private readonly Color _highlightColor = new(0, 1, 0, 0.7f);
    private readonly Color _defaultColor = new(0, 0, 1, 0.5f);
    private readonly Color _cantPlaceColor = new(1, 0, 0, 0.9f);
    private List<GridTile> _currentHighlightedTiles = new List<GridTile>();
    private GridSelectionManager _gridSelectionManager;
    private bool _canPlace = true;
    private int _gridX;
    private int _gridY;
    Block _blockHolding;
    

    void Start()
    {
        _gridSelectionManager = FindObjectOfType<GridSelectionManager>();
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
        _gridSelectionManager.SetBlockOver(null);

<<<<<<< HEAD
        GridVisualizer gridVisualizer = GetComponentInParent<GridVisualizer>();
        GridTile[,] grid = gridVisualizer.GetGrid();

        //Bens AWESOME changes
        if (_renderer != null) _renderer.material.color = GridSelectionManager.Instance.GetHighlightColor();
        Block selectedBlock = FindObjectOfType<GridSelectionManager>().GetCurrentPrefab();
=======
        Block selectedBlock = _gridSelectionManager.GetCurrentPrefab();
>>>>>>> 1c9a93a (move and delete modes!)
        if (selectedBlock == null) return;

        List<Vector2> blockNeighbors = selectedBlock.GetNeighbors();
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        Block[] blockComponents = GetBlockComponents(blocks);
        Color colorToSet = _cantPlaceColor;

        if (_gridSelectionManager.IsMoveToolEnabled() && _gridSelectionManager.IsPlacingBlock() == false || _gridSelectionManager.IsDeleteModeEnabled())
        {
            HandleMoveTool();
        }
        else
        {
            HandlePlacementTool(selectedBlock, blockComponents, blockNeighbors, colorToSet);
        }
    }

    private void HandleMoveTool()
    {
        if (_blockHolding != null)
        {
            HandleBlockHolding(_highlightColor);
        }

        Block selectedBlock = GetBlockUnderMouse();
        if (selectedBlock == null) return;

        HighlightBlockNeighborTiles(selectedBlock, _cantPlaceColor);
        HighlightCurrentTile();
    }

    private void HandlePlacementTool(Block selectedBlock, Block[] blockComponents, List<Vector2> blockNeighbors, Color colorToSet)
    {
        if (_blockHolding != null)
        {
            HandleBlockHolding(colorToSet);
        }

        if (selectedBlock.GetBlockType() == BlockType.SQUARE)
        {
            if (IsSquareBlockOverlapping(blockComponents))
            {
<<<<<<< HEAD
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
=======
>>>>>>> 1c9a93a (move and delete modes!)
                _canPlace = false;
                _renderer.material.color = _cantPlaceColor;
                return;
            }
        }

        HighlightNeighborTiles(blockNeighbors, colorToSet);
        HighlightCurrentTile();
    }

    private Block GetBlockUnderMouse()
    {
        return _gridSelectionManager.GetTileOver().GetBlockHolding();
    }

    private void HighlightCurrentTile()
    {
        if (_renderer == null) return;
        _currentHighlightedTiles.Add(this);

        if (_gridSelectionManager.IsMoveToolEnabled() && _gridSelectionManager.IsPlacingBlock() == false || _gridSelectionManager.IsDeleteModeEnabled())
        {
            _renderer.material.color = _cantPlaceColor;
        }
        else
        {
            _renderer.material.color = _canPlace ? _highlightColor : _cantPlaceColor;
        }
    }

    private Block[] GetBlockComponents(GameObject[] blocks)
    {
        Block[] blockComponents = new Block[blocks.Length];
        for (int i = 0; i < blocks.Length; i++)
        {
            blockComponents[i] = blocks[i].GetComponent<Block>();
        }
        return blockComponents;
    }

    private bool IsSquareBlockOverlapping(Block[] blockComponents)
    {
        foreach (Block block in blockComponents)
        {
            if (block == null) continue;
            List<Vector2> gridPositions = block.GetGridPositions();
            if (gridPositions == null) continue;
            foreach (Vector2 gridPosition in gridPositions)
            {
                if (gridPosition == new Vector2(_gridX, _gridY))
                {
<<<<<<< HEAD
                    Renderer tileRenderer = tile.GetComponent<Renderer>();
                    if (tileRenderer != null)
                    {
                        tileRenderer.material.color = GridSelectionManager.Instance.GetHighlightColor();
                    }
=======
                    return true;
>>>>>>> 1c9a93a (move and delete modes!)
                }
            }
        }
        return false;
    }

    private void HandleBlockHolding(Color colorToSet)
    {
        _gridSelectionManager.SetBlockOver(_blockHolding);
        _canPlace = false;
        _renderer.material.color = colorToSet;
    }

    private void HighlightNeighborTiles(List<Vector2> blockNeighbors, Color colorToSet)
    {
        GridVisualizer gridVisualizer = GetComponentInParent<GridVisualizer>();
        GridTile[,] grid = gridVisualizer.GetGrid();

        List<GridTile> previousNeighbors = new List<GridTile>();
        List<GridTile> neighborsChecked = new List<GridTile>();

        foreach (Vector2 neighbor in blockNeighbors)
        {
            int neighborGridX = _gridX + (int)neighbor.x;
            int neighborGridY = _gridY + (int)neighbor.y;

            if (IsTileOutOfBounds(neighborGridX, neighborGridY, gridVisualizer))
            {
                HighlightPreviousNeighbors(previousNeighbors, _cantPlaceColor);
                _canPlace = false;
                _renderer.material.color = _cantPlaceColor;
                continue;
            }

            GridTile tile = grid[neighborGridX, neighborGridY];
            neighborsChecked.Add(tile);

            if (!_canPlace)
            {
                HighlightTile(tile, colorToSet);
                continue;
            }

            if (tile._blockHolding != null)
            {
                _canPlace = false;
                HighlightCheckedNeighbors(neighborsChecked, colorToSet);
                HighlightTile(tile, colorToSet);
                continue;
            }

            HighlightTile(tile, _highlightColor);
            previousNeighbors.Add(tile);
        }
    }

    private void HighlightBlockNeighborTiles(Block block, Color colorToSet)
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("GridTile");
        foreach (GameObject tile in tiles)
        {
            GridTile gridTile = tile.GetComponent<GridTile>();
            if (gridTile == null) continue;
            if (gridTile.GetBlockHolding() == block)
            {
                gridTile._renderer.material.color = colorToSet;
                _currentHighlightedTiles.Add(gridTile);
            }
        }
    }

    private bool IsTileOutOfBounds(int x, int y, GridVisualizer gridVisualizer)
    {
        return x < 0 || x >= gridVisualizer.GetWidth() || y < 0 || y >= gridVisualizer.GetHeight();
    }

    private void HighlightPreviousNeighbors(List<GridTile> previousNeighbors, Color color)
    {
        foreach (GridTile prevTile in previousNeighbors)
        {
            prevTile.GetComponent<Renderer>().material.color = color;
            _currentHighlightedTiles.Add(prevTile);
        }
    }

    private void HighlightCheckedNeighbors(List<GridTile> neighborsChecked, Color color)
    {
        foreach (GridTile neighborTile in neighborsChecked)
        {
            neighborTile._renderer.material.color = color;
            _currentHighlightedTiles.Add(neighborTile);
        }
    }

    private void HighlightTile(GridTile tile, Color color)
    {
        if (tile != null && tile._renderer != null)
        {
            tile._renderer.material.color = color;
            _currentHighlightedTiles.Add(this);
        }
    }

    public List<GridTile> GetTileNeighbors(List<Vector2> blockNeighbors)
    {
        GridVisualizer gridVisualizer = GetComponentInParent<GridVisualizer>();
        List<GridTile> tileNeighbors = new List<GridTile>();
        foreach (Vector2 neighbor in blockNeighbors)
        {
            int x = _gridX + (int)neighbor.x;
            int y = _gridY + (int)neighbor.y;
            tileNeighbors.Add(gridVisualizer.GetGrid()[x, y]);
        }
        return tileNeighbors;
    }

    public static void UnhighlightTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("GridTile");
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<GridTile>().SetColor(new Color(0, 0, 1, 0.5f));
            tile.GetComponent<GridTile>().ClearHighlightedTiles();
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
        _gridX = x;
        _gridY = y;
    }

    public Vector2 GetGridPosition()
    {
        return new Vector2(_gridX, _gridY);
    }

    public Block GetBlockHolding() { return _blockHolding; }

    public void SetBlockHolding(Block block) { _blockHolding = block; }

    public List<GridTile> GetCurrentHighlightedTiles() { return _currentHighlightedTiles; }

    public void ClearHighlightedTiles() { _currentHighlightedTiles.Clear(); }

    public bool GetCanPlace() { return _canPlace; }
}
