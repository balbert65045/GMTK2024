using System.Collections.Generic;
using UnityEngine;

public class GridSelectionManager : MonoBehaviour
{
    private GridTile _tileOver = null;
    private Block _blockOver = null;
    [SerializeField] private bool _moveToolEnabled = false;
    [SerializeField] private bool _deleteModeEnabled = false;
    private bool _placingBlock = false;

    public GameObject _prefabBlockPlacing = null;
    int webCostForBlock = 0;
    private Block _blockMoving;
    public Block GetBlockMoving() { return _blockMoving; }

    private readonly Color _highlightColor = new(0, 1, 0, 0.7f);
    private readonly Color _cantPlaceColor = new(1, 0, 0, 0.9f);

    public static GridSelectionManager Instance;
    [SerializeField] InventoryController inventoryController;


    public GridTile lastGoodMovingTile;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        inventoryController.SelectedBlockTypeChangedEvent?.AddListener(SetCurrentPlacingBlock);
        GameManager.Instance.GameModeChangedEvent?.AddListener(OnGameModeChanged);
    }

    void OnGameModeChanged(GameMode gameMode)
    {
        if (gameMode == GameMode.PLATFORM_MODE)
        {
            ClearHighlights();
        }
    }

    public void SetCurrentPlacingBlock(BlockType blockType)
    {
        Debug.Log(blockType);
        BlockSO soUsing = null;
        foreach (BlockSO so in InventoryController.Instance.DefaultBlocks)
        {
            if (so.Type == blockType)
            {
                soUsing = so;
                break;
            }
        }
        if (soUsing.Prefab == null)
        {
            Debug.LogError("No prefab for the scriptable object or no matching type");
            return;
        }
        _prefabBlockPlacing = soUsing.Prefab;
        webCostForBlock = soUsing.Cost;
        if (_tileOver) { _tileOver.HighlightTiles(_prefabBlockPlacing.GetComponent<Block>()); }
    }

    public Color GetHighlightColor()
    {
        if (webCostForBlock > WebResourceController.Instance.GetWebCount())
        {
            return _cantPlaceColor;
        }
        return _highlightColor;
    }

    public void CheckToPlaceMovingObject()
    {
        if (_tileOver == null) { return; }
        if (!_tileOver.CanPutBlockHere(_blockMoving)) { return; }
        lastGoodMovingTile = _tileOver;
        _blockMoving.transform.position = _tileOver.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.Mode == GameMode.BUILD_MODE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_tileOver == null) return;
                if (_tileOver.GetBlockHolding() != null)
                {
                    _blockMoving = _tileOver.GetBlockHolding();
                    _blockMoving.Moving = true;
                    foreach (GridTile tile in _blockMoving.tilesCurrentlyOn)
                    {
                        tile.SetBlockHolding(null);
                        tile.SetCanPlace(true);
                    }
                    _prefabBlockPlacing = null;
                    lastGoodMovingTile = _tileOver;
                }
                else
                {
                    if (webCostForBlock > WebResourceController.Instance.GetWebCount()) { return; }
                    if (_prefabBlockPlacing == null) return;
                    bool canPlace = _tileOver.GetCanPlace();
                    if (!canPlace) return;
                    //NEED to check neighbors as well here!
                    GameObject block = Instantiate(_prefabBlockPlacing);
                    block.GetComponent<Block>().SetCost(webCostForBlock);
                    block.transform.position = _tileOver.transform.position;
                    WebResourceController.Instance.DecrementWebCount(webCostForBlock);
                    SetBlockOnTile(block.GetComponent<Block>());
                    //SetBlockOnTile(block.GetComponent<Block>());
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_blockMoving != null)
                {
                    SetBlockOnLastGoodTile(_blockMoving.GetComponent<Block>());
                    _blockMoving = null;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (_tileOver != null && _tileOver.GetBlockHolding() != null)
                {
                    Block block = _tileOver.GetBlockHolding();
                    foreach (GridTile tile in block.tilesCurrentlyOn)
                    {
                        tile.SetBlockHolding(null);
                        tile.SetCanPlace(true);
                    }
                    WebResourceController.Instance.IncrementWebCount(block.GetCost());
                    Destroy(block.gameObject);
                }
            }
                /*
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
                */
            

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

    void SetBlockOnLastGoodTile(Block block)
    {
        if(lastGoodMovingTile == null || block == null) return;
        lastGoodMovingTile.SetBlockHolding(block);
        block.ClearTiles();
        block.AddTile(lastGoodMovingTile);
        List<GridTile> tileNeighbors = lastGoodMovingTile.GetTileNeighbors(block.GetNeighbors());
        foreach (GridTile tile in tileNeighbors)
        {
            block.AddTile(tile);
            tile.SetBlockHolding(block);
        }
    }

    public void SetBlockOnTile(Block block)
    {
        if (_tileOver == null || block == null) return;
        _tileOver.SetBlockHolding(block);
        block.ClearTiles();
        block.AddTile(_tileOver);
        List<GridTile> tileNeighbors = _tileOver.GetTileNeighbors(block.GetNeighbors());
        foreach (GridTile tile in tileNeighbors)
        {
            block.AddTile(tile);
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
       // _currentPrefab = BlockPrefab.GetComponent<Block>();
        if (_tileOver) _tileOver.HighlightTiles(BlockPrefab.GetComponent<Block>()); 
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

    public GameObject GetCurrentPrefab()
    {
        return _prefabBlockPlacing;
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
