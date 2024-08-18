using System.Collections.Generic;
using UnityEngine;

public class GridSelectionManager : MonoBehaviour
{
    private GridTile _tileOver = null;
    private Block _blockOver = null;
    private Block _currentPrefab = null;
    [SerializeField] private bool _moveToolEnabled = false;
    [SerializeField] private bool _deleteModeEnabled = false;
    private bool _placingBlock = false;

    private Block _blockMoving;

    
    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.Mode == GameMode.BUILD_MODE)
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                if (_moveToolEnabled && !_placingBlock) 
                {
                    if (_blockOver == null) return;
                    _blockMoving = _blockOver;
                    ClearBlockOnTile(_blockOver);
                    _blockMoving.gameObject.SetActive(false);
                    _placingBlock = true;
                    _currentPrefab = _blockMoving;
                }
                else if (_moveToolEnabled && _placingBlock)
                {
                    if (_tileOver == null) return;

                    bool canPlace = _tileOver.GetCanPlace();
                    if (!canPlace) return;

                    _blockMoving.transform.position = _tileOver.transform.position;
                    _blockMoving.gameObject.SetActive(true);
                    SetBlockOnTile(_blockMoving, _blockMoving);
                    _placingBlock = false;
                    _blockMoving = null;
                }
                else if (_deleteModeEnabled)
                {
                    if (_blockOver == null) return;
                    ClearBlockOnTile(_blockOver);
                    Destroy(_blockOver.gameObject);
                }
                else
                {
                    if (_currentPrefab == null || _tileOver == null) return;

                    bool canPlace = _tileOver.GetCanPlace();
                    if (!canPlace) return;

                    GameObject block = Instantiate(_currentPrefab.gameObject);
                    block.transform.position = _tileOver.transform.position;
                    SetBlockOnTile(block.GetComponent<Block>(), _currentPrefab);
                }

                GridTile.UnhighlightTiles();
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                _moveToolEnabled = false;
                _deleteModeEnabled = !_deleteModeEnabled;
            }

            //temporary code to test move tool
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_blockMoving != null) return;
                _deleteModeEnabled = false;
                _moveToolEnabled = !_moveToolEnabled;
                ClearHighlights();
            }
            
                    
                

                // create move tool
                //Would love to be able to drag and move blocks. Use GetMouseButtonUp

                //Right click to delete would also be great
            
            
                    
                

                // create move tool
                //Would love to be able to drag and move blocks. Use GetMouseButtonUp

                //Right click to delete would also be great

            
        }
    }

    public void SetBlockOnTile(Block block, Block prefab)
    {
        if (_tileOver == null || block == null || prefab == null) return;

        _tileOver.SetBlockHolding(block);
        List<GridTile> tileNeighbors = _tileOver.GetTileNeighbors(prefab.GetNeighbors());
        foreach (GridTile tile in tileNeighbors)
        {
            tile.SetBlockHolding(block);
        }
    }

    public void ClearBlockOnTile(Block block)
    {
        if (_tileOver == null || block == null) return;

        GameObject[] tiles = GameObject.FindGameObjectsWithTag("GridTile");
        foreach (GameObject tile in tiles)
        {
            GridTile gridTile = tile.GetComponent<GridTile>();
            if (gridTile.GetBlockHolding() == block)
            {
                gridTile.SetBlockHolding(null);
            }
        }
    }

    public void SetCurrentPlacingBlock(GameObject BlockPrefab)
    {
        _currentPrefab = BlockPrefab.GetComponent<Block>();
        if (_tileOver) _tileOver.HighlightTiles(); 
    }

    public void SetTileOver(GridTile tile)
    {
        _tileOver = tile;
    }

    public GridTile GetTileOver()
    {
        return _tileOver;
    }

    public void SetBlockOver(Block block)
    {
        _blockOver = block;
    }

    public Block GetBlockOver()
    {
        return _blockOver;
    }

    public Block GetCurrentPrefab()
    {
        return _currentPrefab;
    }

    public List<GridTile> GetHighlightedTiles()
    {
        return _tileOver.GetCurrentHighlightedTiles();
    }

    public bool IsMoveToolEnabled()
    {
        return _moveToolEnabled;
    }

    public bool IsDeleteModeEnabled()
    {
        return _deleteModeEnabled;
    }

    public bool IsPlacingBlock()
    {
        return _placingBlock;
    }

    public void ClearHighlights()
        {
            GameObject[] tiles = GameObject.FindGameObjectsWithTag("GridTile");
            foreach (GameObject tile in tiles)
            {
                tile.GetComponent<GridTile>().SetColor(new Color(0, 0, 1, 0.5f));
            }
        }
}
