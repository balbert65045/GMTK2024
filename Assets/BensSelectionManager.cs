using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BensSelectionManager : MonoBehaviour
{
    public GridTile _tileOver = null;
    public Block _prefabBlockPlacing = null;
    int webCostForBlock = 0;
    [SerializeField] InventoryController inventoryController;
    Block blockMoving;

    private readonly Color _highlightColor = new(0, 1, 0, 0.7f);
    private readonly Color _cantPlaceColor = new(1, 0, 0, 0.9f);

    public Color GetHighlightColor()
    {
        if (webCostForBlock > WebResourceController.Instance.GetWebCount())
        {
            return _cantPlaceColor;
        }
        return _highlightColor;
    }

    public static BensSelectionManager Instance;

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

    public void ClearHighlights()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("GridTile");
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<GridTile>().SetColor(new Color(0, 0, 1, 0.5f));
        }
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
        _prefabBlockPlacing = soUsing.Prefab.GetComponent<Block>();
        webCostForBlock = soUsing.Cost;
        if (_tileOver) { _tileOver.HighlightTiles(); }
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.Mode == GameMode.BUILD_MODE)
        {
            if (webCostForBlock > WebResourceController.Instance.GetWebCount()) { return; }
            if (Input.GetMouseButtonDown(0))
            {
                if (_prefabBlockPlacing == null) return;
                if(_tileOver == null) return; 
                
                if (_tileOver.GetBlockHolding() != null)
                {
                    blockMoving = _tileOver.GetBlockHolding();
                }
                else {
                    bool canPlace = _tileOver.GetCanPlace();
                    if (!canPlace) return;
                    //NEED to check neighbors as well here!
                    GameObject block = Instantiate(_prefabBlockPlacing.gameObject);
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
        _tileOver.SetBlockHolding(block.GetComponent<Block>());
        List<GridTile> tileNeighbors = _tileOver.GetTileNeighbors(_prefabBlockPlacing.GetNeighbors());
        foreach (GridTile tile in tileNeighbors)
        {
            tile.SetBlockHolding(block.GetComponent<Block>());
        }
    }

    public void SetCurrentPlacingBlock(GameObject BlockPrefab)
    {
        _prefabBlockPlacing = BlockPrefab.GetComponent<Block>();
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
        return _prefabBlockPlacing;
    }

}
