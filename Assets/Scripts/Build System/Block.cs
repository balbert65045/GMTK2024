using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType {
    SQAURE,
    VERTICAL_THREE,
    HORIZONTAL_THREE,
    CROSS,
    L_SHAPE_LEFT,
    L_SHAPE_RIGHT,
}

public class Block : MonoBehaviour
{
    [SerializeField] private bool _isSelected = true;
    public BlockType _blockType;
    private bool _tileUp = false;
    private bool _tileDown = false;
    private bool _tileLeft = false;
    private bool _tileRight = false;
    private bool _tileUpLeft = false;
    private bool _tileUpRight = false;
    private bool _tileDownLeft = false;
    private bool _tileDownRight = false;

    private bool _isDragging = false;
    private GameObject[] _gridTiles;

    void Start()
    {

        switch(_blockType)
        {
            case BlockType.SQAURE:
                _tileUp = false;
                _tileDown = false;
                _tileLeft = false;
                _tileRight = false;
                break;
            case BlockType.VERTICAL_THREE:
                _tileUp = true;
                _tileDown = true;
                break;
            case BlockType.HORIZONTAL_THREE:
                _tileLeft = true;
                _tileRight = true;
                break;
            case BlockType.CROSS:
                _tileUp = true;
                _tileDown = true;
                _tileLeft = true;
                _tileRight = true;
                break;
            case BlockType.L_SHAPE_LEFT:
                _tileUp = true;
                _tileDown = true;
                _tileDownLeft = true;
                break;
            case BlockType.L_SHAPE_RIGHT:
                _tileUp = true;
                _tileDown = true;
                _tileDownRight = true;
                break;
        }

        GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.5f);

        StartCoroutine(InitGridTiles());
        
    }

    void Update()
    {
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

    void FindSelectedTile()
    {
        foreach (GameObject gridTile in _gridTiles)
        {
            GridTile gridScript = gridTile.GetComponent<GridTile>();
            bool isSelected = gridScript.IsSelected();
            bool canPlace = gridScript.CanPlace();
            if (isSelected && canPlace)
            {
                // set the selected tile to the grid tile
                transform.position = gridTile.transform.position;
            }
        }       
    }

    IEnumerator InitGridTiles()
    {
        yield return new WaitForSeconds(0.5f);
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
}
