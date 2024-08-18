using System.Collections.Generic;
using UnityEngine;

public class GridSelectionManager : MonoBehaviour
{
    public GridTile tileOver = null;
    public Block prefabBlockPlacing = null;
    int webCostForBlock = 0;
    [SerializeField] InventoryController inventoryController;
    Block blockMoving;

    private void Start()
    {
        inventoryController.SelectedBlockTypeChangedEvent?.AddListener(SetCurrentPlacingBlock);
    }

    public void SetCurrentPlacingBlock(BlockType blockType)
    {
        BlockSO soUsing = null;
        foreach(BlockSO so in InventoryController.Instance.DefaultBlocks)
        {
            if (so.Type == blockType) {
                soUsing = so;
                break;
            }
        }
        if(soUsing.Prefab == null)
        {
            Debug.LogError("No prefab for the scriptable object or no matching type");
            return;
        }
        prefabBlockPlacing = soUsing.Prefab.GetComponent<Block>();
        webCostForBlock = soUsing.Cost;
        if (tileOver) { tileOver.HighlightTiles(); }
    }
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
        WebResourceController.Instance.DecrementWebCount(webCostForBlock);
        tileOver.SetBlockHolding(block.GetComponent<Block>());
        List<GridTile> tileNeighbors = tileOver.GetTileNeighbors(prefabBlockPlacing.GetNeighbors());
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
