using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{

    private Renderer _renderer;
    private bool _isSelected = false;
    private bool _canPlace = true;
    private readonly Color _highlightColor = new(0, 1, 0, 0.7f);
    private readonly Color _defaultColor = new(0, 0, 1, 0.5f);

    private GridTile _leftTile;
    private GridTile _rightTile;
    private GridTile _topTile;
    private GridTile _bottomTile;
    private GridTile _topLeftTile;
    private GridTile _topRightTile;
    private GridTile _bottomLeftTile;
    private GridTile _bottomRightTile;
    private Block _currentlySelectedBlock;
    private int _x;
    private int _y;
    //private Dictionary<string, bool> _neighboringTiles;

    void Start()
    {
        // set the layer to Grid
        gameObject.layer = LayerMask.NameToLayer("Grid");
        gameObject.tag = "GridTile";
        // store reference of the block
        FindSelectedBlock();

        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
        {
            _renderer.material.shader = Shader.Find("Transparent/Diffuse");
            _renderer.material.color = _defaultColor;
        }
    }

    public void FindSelectedBlock()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in blocks)
        {
            Block blockScript = block.GetComponent<Block>();
            if (blockScript.IsSelected())
            {
                _currentlySelectedBlock = blockScript;
            }
        }

        if (_renderer != null) _renderer.material.color = _defaultColor;
    }

    void HighlightTiles()
    {
        _canPlace = true;
        GridVisualizer gridVisualizer = GetComponentInParent<GridVisualizer>();
        GridTile[,] grid = gridVisualizer.GetGrid();
        List<Vector2> blockNeighbors = _currentlySelectedBlock.GetNeighbors();
        foreach (Vector2 neighbor in blockNeighbors)
        {
            int x = _x + (int)neighbor.x;
            int y = _y + (int)neighbor.y;
            if (x < 0 || x >= gridVisualizer.GetWidth() || y < 0 || y >= gridVisualizer.GetHeight())
            {
                _canPlace = false;
                return;
            }
            else if (x >= 0 && x < gridVisualizer.GetWidth() && y >= 0 && y < gridVisualizer.GetHeight())
            {
                GridTile tile = grid[x, y];
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

    void UnhighlightTiles()
    {
        GridVisualizer gridVisualizer = GetComponentInParent<GridVisualizer>();
        GridTile[,] grid = gridVisualizer.GetGrid();
        List<Vector2> blockNeighbors = _currentlySelectedBlock.GetNeighbors();
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
        if (_renderer != null) _renderer.material.color = _highlightColor;
        HighlightTiles();
        _isSelected = true;
    }

    void OnMouseExit()
    {
        if (_renderer != null) _renderer.material.color = _defaultColor;
        UnhighlightTiles();
        _isSelected = false;
    }

    void FindNeighboringTiles()
    {
        _leftTile = GetTileAtPosition(Vector3.left);
        _rightTile = GetTileAtPosition(Vector3.right);
        _topTile = GetTileAtPosition(Vector3.up);
        _bottomTile = GetTileAtPosition(Vector3.down);
        _topLeftTile = GetTileAtPosition(Vector3.up + Vector3.left);
        _topRightTile = GetTileAtPosition(Vector3.up + Vector3.right);
        _bottomLeftTile = GetTileAtPosition(Vector3.down + Vector3.left);
        _bottomRightTile = GetTileAtPosition(Vector3.down + Vector3.right);
    }

    GridTile GetTileAtPosition(Vector3 direction)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, direction, out hit, 1.0f);
        Debug.DrawRay(transform.position, direction, Color.red, 100.0f);
        if (hit.collider != null)
        {
            return hit.collider.gameObject.GetComponent<GridTile>();
        }
        else
        {
            return null;
        }
    }


    public bool IsSelected()
    {
        return _isSelected;
    }

    public bool CanPlace()
    {
        return _canPlace;
    }

    public void SetGridPosition(int x, int y)
    {
        _x = x;
        _y = y;
    }
}
