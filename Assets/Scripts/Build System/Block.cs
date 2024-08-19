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
    [SerializeField] private BlockType _blockType;
    [SerializeField] private bool _isStaticBlock = false;
    private List<Vector2> _gridPositions = new List<Vector2>();
    private GridSelectionManager _gridSelectionManager;

    public List<GridTile> tilesCurrentlyOn = new List<GridTile>();
    public void AddTile(GridTile tile) { tilesCurrentlyOn.Add(tile); }
    public void ClearTiles() { tilesCurrentlyOn.Clear(); }

    public bool Moving = false;

    int Cost = 0;
    public void SetCost(int cost) { Cost = cost; }
    public int GetCost() { return Cost; }


    void Start()
    {
        if (!_isStaticBlock)
        {
            _gridSelectionManager = FindObjectOfType<GridSelectionManager>();
            GridTile gridTile = _gridSelectionManager.GetTileOver();
            List<GridTile> selectedTiles = gridTile.GetCurrentHighlightedTiles();

            if (_blockType == BlockType.SQUARE)
            {
                _gridPositions.Add(gridTile.GetGridPosition());
            }
            else
            {
                foreach (GridTile tile in selectedTiles)
                {
                    _gridPositions.Add(tile.GetGridPosition());
                }
            }
        }
        else
        {

            

        }
    }

    public List<Vector2> GetNeighbors()
    {
        List<Vector2> _neighbors = new List<Vector2>();

        switch (_blockType)
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

        return _neighbors;
    }

    public BlockType GetBlockType() { return _blockType; }

    public List<Vector2> GetGridPositions()
    {
        return _gridPositions;
    }

    public void AddGridPosition (Vector2 gridPosition)
    {
        _gridPositions.Add(gridPosition);
    }

    public void SetGridPositions(List<Vector2> gridPositions)
    {
        _gridPositions = gridPositions;
    }

    public void ClearGridPositions()
    {
        _gridPositions.Clear();
    }

    public bool IsBlockStatic()
    {
        return _isStaticBlock;
    }

    public void SetStaticPosition()
    {
        GameObject[] gridTiles = GameObject.FindGameObjectsWithTag("GridTile");
        foreach (GameObject gridTile in gridTiles)
        {
            GridTile tile = gridTile.GetComponent<GridTile>();
            if (tile.transform.position == transform.position)
            {
                Debug.Log("Found tile");
                tile.SetBlockHolding(this);

                List<GridTile> neighbors = tile.GetTileNeighbors(GetNeighbors());
                foreach (GridTile neighbor in neighbors)
                {
                    neighbor.SetBlockHolding(this);
                }
            }
        }
    }
}
