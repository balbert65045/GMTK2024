using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType {
    SQUARE,
    VERTICAL_THREE,
    HORIZONTAL_THREE,
    CROSS,
    L_SHAPE_LEFT,
    L_SHAPE_RIGHT,
}

public class Block : MonoBehaviour
{
    [SerializeField] private bool _isSelected = true;
    public BlockType blockType;
    /*
    private bool _tileUp = false;
    private bool _tileDown = false;
    private bool _tileLeft = false;
    private bool _tileRight = false;
    private bool _tileUpLeft = false;
    private bool _tileUpRight = false;
    private bool _tileDownLeft = false;
    private bool _tileDownRight = false;
    */

    private List<Vector2> _neighbors = new List<Vector2>();

    private bool _isDragging = false;
    private bool _isColliding = false;
    private GameObject[] _gridTiles;

    void Start()
    {

        switch(blockType)
        {
            case BlockType.VERTICAL_THREE:
                _neighbors.Add(new Vector2(0, 1));
                _neighbors.Add(new Vector2(0, -1));
                break;
            case BlockType.HORIZONTAL_THREE:
                _neighbors.Add(new Vector2(1, 0));
                _neighbors.Add(new Vector2(-1, 0));
                break;
            case BlockType.CROSS:
                _neighbors.Add(new Vector2(0, 1));
                _neighbors.Add(new Vector2(0, -1));
                _neighbors.Add(new Vector2(1, 0));
                _neighbors.Add(new Vector2(-1, 0));
                break;
            case BlockType.L_SHAPE_LEFT:
                _neighbors.Add(new Vector2(0, 1));
                _neighbors.Add(new Vector2(0, -1));
                _neighbors.Add(new Vector2(-1, -1));
                break;
            case BlockType.L_SHAPE_RIGHT:
                _neighbors.Add(new Vector2(0, 1));
                _neighbors.Add(new Vector2(0, -1));
                _neighbors.Add(new Vector2(1, -1));
                break;
        }

        GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.5f);

        StartCoroutine(InitGridTiles());
        
    }

    void Update()
    {
        if (GameManager.Instance.Mode != GameMode.BUILD_MODE) { return; }
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }

        if (_isDragging && _isSelected)
        {
            FindSelectedTile();
        }
    }

    public void FindSelectedTile()
    {
        _gridTiles = GameObject.FindGameObjectsWithTag("GridTile");
        foreach (GameObject gridTile in _gridTiles)
        {
            GridTile gridScript = gridTile.GetComponent<GridTile>();
            bool isSelected = gridScript.IsSelected();
            bool canPlace = gridScript.CanPlace();
            if (isSelected && canPlace)
            {
                // set the selected tile to the grid tile
                transform.position = gridTile.transform.position;
                /*
                    currentPosition = gridTile.transform.position;
                */
            }
        }       
    }

    void CheckIfColliding()
    {
        
    }

    IEnumerator InitGridTiles()
    {
        yield return new WaitForSeconds(0.1f);
        _gridTiles = GameObject.FindGameObjectsWithTag("GridTile");
    }

    public bool IsSelected()
    {
        return _isSelected;
    }

    public void SetSelected(bool isSelected)
    {
        _isSelected = isSelected;
    }
    /*
    public Dictionary<string, bool> GetTiles()
    {
        Dictionary<string, bool>  tiles = new Dictionary<string, bool>
        {
            { "Up", _tileUp },
            { "Down", _tileDown },
            { "Left", _tileLeft },
            { "Right", _tileRight },
            { "UpLeft", _tileUpLeft },
            { "UpRight", _tileUpRight },
            { "DownLeft", _tileDownLeft },
            { "DownRight", _tileDownRight }
        };
        return tiles;
    }
    */

    public void SelectBlock()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in blocks)
        {
            Block blockScript = block.GetComponent<Block>();
            blockScript.SetSelected(false);
        }
        _isSelected = true;
        GameObject[] gridTiles = GameObject.FindGameObjectsWithTag("GridTile");
        foreach (GameObject gridTile in gridTiles)
        {
            GridTile gridScript = gridTile.GetComponent<GridTile>();
            gridScript.FindSelectedBlock();
        }
    }

    public List<Vector2> GetNeighbors()
    {
        return _neighbors;
    }
}
