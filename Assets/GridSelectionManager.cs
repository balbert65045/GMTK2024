using System.Collections.Generic;
using UnityEngine;

public class GridSelectionManager : MonoBehaviour
{
    private GridTile _tileOver = null;
    private Block _currentPrefab = null;

    Block blockMoving;

    
    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.Mode == GameMode.BUILD_MODE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_currentPrefab == null) return;
                if(_tileOver == null) return; 
                
                if (_tileOver.GetBlockHolding() != null)
                {
                    blockMoving = _tileOver.GetBlockHolding();
                }
                else {
                    bool canPlace = _tileOver.GetCanPlace();
                    if (!canPlace) return;
                    //NEED to check neighbors as well here!
                    GameObject block = Instantiate(_currentPrefab.gameObject);
                    block.transform.position = _tileOver.transform.position;
                    SetBlockOnTile(block.GetComponent<Block>());
                }


                //Would love to be able to drag and move blocks. Use GetMouseButtonUp

                //Right click to delete would also be great
            }
        }
    }
    

    public void SetBlockOnTile(Block block)
    {
        _tileOver.SetBlockHolding(block.GetComponent<Block>());
        List<GridTile> tileNeighbors = _tileOver.GetTileNeighbors(_currentPrefab.GetNeighbors());
        foreach (GridTile tile in tileNeighbors)
        {
            tile.SetBlockHolding(block.GetComponent<Block>());
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

    public Block GetCurrentPrefab()
    {
        return _currentPrefab;
    }

}
