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
    private Dictionary<string, bool> _neighboringTiles;

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
        FindNeighboringTiles();
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
                _neighboringTiles = _currentlySelectedBlock.GetTiles();
            }
        }

        if (_renderer != null) _renderer.material.color = _defaultColor;
    }

    void HighlightTiles()
    {
        _canPlace = true;
        if (_neighboringTiles != null)
        {
            foreach (KeyValuePair<string, bool> tile in _neighboringTiles)
            {
                switch (tile.Key)
                {
                    case "Up":
                    if (_topTile && tile.Value)
                    {
                        Renderer topTileRenderer = _topTile.GetComponent<Renderer>();
                        if (topTileRenderer != null)
                        {
                            topTileRenderer.material.color = _highlightColor;
                        }
                    }
                    else if (!_topTile && tile.Value)
                    {
                        _canPlace = false;
                    }
                    break;
                case "Down":
                    if (_bottomTile && tile.Value)
                    {
                        Renderer bottomTileRenderer = _bottomTile.GetComponent<Renderer>();
                        if (bottomTileRenderer != null)
                        {
                            bottomTileRenderer.material.color = _highlightColor;
                        }
                    } 
                    else if (!_bottomTile && tile.Value)
                    {
                        _canPlace = false;
                    }
                    break;
                case "Left":
                    if (_leftTile && tile.Value)
                    {
                        Renderer leftTileRenderer = _leftTile.GetComponent<Renderer>();
                        if (leftTileRenderer != null)
                        {
                            leftTileRenderer.material.color = _highlightColor;
                        }
                    } 
                    else if (!_leftTile && tile.Value)
                    {
                        _canPlace = false;
                    }
                    break;
                case "Right":
                    if (_rightTile && tile.Value)
                    {
                        Renderer rightTileRenderer = _rightTile.GetComponent<Renderer>();
                        if (rightTileRenderer != null)
                        {
                            rightTileRenderer.material.color = _highlightColor;
                        }
                    } 
                    else if (!_rightTile && tile.Value)
                    {
                        _canPlace = false;
                    }
                    break;
                case "UpLeft":
                    if (_topLeftTile && tile.Value)
                    {
                        Renderer topLeftTileRenderer = _topLeftTile.GetComponent<Renderer>();
                        if (topLeftTileRenderer != null)
                        {
                            topLeftTileRenderer.material.color = _highlightColor;
                        }
                    } 
                    else if (!_topLeftTile && tile.Value)
                    {
                        _canPlace = false;
                    }
                    break;
                case "UpRight":
                    if (_topRightTile && tile.Value)
                    {
                        Renderer topRightTileRenderer = _topRightTile.GetComponent<Renderer>();
                        if (topRightTileRenderer != null)
                        {
                            topRightTileRenderer.material.color = _highlightColor;
                        }
                    } 
                    else if (!_topRightTile && tile.Value)
                    {
                        _canPlace = false;
                    }
                    break;
                case "DownLeft":
                    if (_bottomLeftTile && tile.Value)
                    {
                        Renderer bottomLeftTileRenderer = _bottomLeftTile.GetComponent<Renderer>();
                        if (bottomLeftTileRenderer != null)
                        {
                            bottomLeftTileRenderer.material.color = _highlightColor;
                        }
                    } 
                    else if (!_bottomLeftTile && tile.Value)
                    {
                        _canPlace = false;
                    }
                    break;
                case "DownRight":
                    if (_bottomRightTile && tile.Value)
                    {
                        Renderer bottomRightTileRenderer = _bottomRightTile.GetComponent<Renderer>();
                        if (bottomRightTileRenderer != null)
                        {
                            bottomRightTileRenderer.material.color = _highlightColor;
                        }
                    } 
                    else if (!_bottomRightTile && tile.Value)
                    {
                        _canPlace = false;
                    }
                    break;
                }
            }
        }
    }

    void UnhighlightTiles()
    {
        if (_neighboringTiles != null)
        {
            foreach (KeyValuePair<string, bool> tile in _neighboringTiles)
            {
                switch (tile.Key)
                {
                    case "Up":
                    if (_topTile && tile.Value)
                    {
                        Renderer topTileRenderer = _topTile.GetComponent<Renderer>();
                        if (topTileRenderer != null)
                        {
                            topTileRenderer.material.color = _defaultColor;
                        }
                    }
                    break;
                case "Down":
                    if (_bottomTile && tile.Value)
                    {
                        Renderer bottomTileRenderer = _bottomTile.GetComponent<Renderer>();
                        if (bottomTileRenderer != null)
                        {
                            bottomTileRenderer.material.color = _defaultColor;
                        }
                    }
                    break;
                case "Left":
                    if (_leftTile && tile.Value)
                    {
                        Renderer leftTileRenderer = _leftTile.GetComponent<Renderer>();
                        if (leftTileRenderer != null)
                        {
                            leftTileRenderer.material.color = _defaultColor;
                        }
                    }
                    break;
                case "Right":
                    if (_rightTile && tile.Value)
                    {
                        Renderer rightTileRenderer = _rightTile.GetComponent<Renderer>();
                        if (rightTileRenderer != null)
                        {
                            rightTileRenderer.material.color = _defaultColor;
                        }
                    }
                    break;
                case "UpLeft":
                    if (_topLeftTile && tile.Value)
                    {
                        Renderer topLeftTileRenderer = _topLeftTile.GetComponent<Renderer>();
                        if (topLeftTileRenderer != null)
                        {
                            topLeftTileRenderer.material.color = _defaultColor;
                        }
                    }
                    break;
                case "UpRight":
                    if (_topRightTile && tile.Value)
                    {
                        Renderer topRightTileRenderer = _topRightTile.GetComponent<Renderer>();
                        if (topRightTileRenderer != null)
                        {
                            topRightTileRenderer.material.color = _defaultColor;
                        }
                    }
                    break;
                case "DownLeft":
                    if (_bottomLeftTile && tile.Value)
                    {
                        Renderer bottomLeftTileRenderer = _bottomLeftTile.GetComponent<Renderer>();
                        if (bottomLeftTileRenderer != null)
                        {
                            bottomLeftTileRenderer.material.color = _defaultColor;
                        }
                    }
                    break;
                case "DownRight":
                    if (_bottomRightTile && tile.Value)
                    {
                        Renderer bottomRightTileRenderer = _bottomRightTile.GetComponent<Renderer>();
                        if (bottomRightTileRenderer != null)
                        {
                            bottomRightTileRenderer.material.color = _defaultColor;
                        }
                    }
                    break;
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
            Debug.Log(hit.collider.gameObject.name);
            return hit.collider.gameObject.GetComponent<GridTile>();
        }
        else
        {
            Debug.Log("No tile found");
        }
        return null;
    }


    public bool IsSelected()
    {
        return _isSelected;
    }

    public bool CanPlace()
    {
        return _canPlace;
    }
}
