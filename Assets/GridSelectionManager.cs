using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSelectionManager : MonoBehaviour
{
    public GridTile tileOver = null;
    public Block prefabBlockPlacing = null;

    Block blockMoving;

    public void SetCurrentPlacingBlock(GameObject BlockPrefab)
    {
        prefabBlockPlacing = BlockPrefab.GetComponent<Block>();
        if (tileOver) { tileOver.HighlightTiles(); }
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.Mode == GameMode.BUILD_MODE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (prefabBlockPlacing == null) { return; }
                if(tileOver == null) { return; } 
                
                if (tileOver.GetBlockHolding() != null)
                {
                    blockMoving = tileOver.GetBlockHolding();
                }
                else {
                    //NEED to check neighbors as well here!


                    GameObject block = Instantiate(prefabBlockPlacing.gameObject);
                    block.transform.position = tileOver.transform.position;
                    SetBlockOnTile(block.GetComponent<Block>());
                }


                //Would love to be able to drag and move blocks. Use GetMouseButtonUp

                //Right click to delete would also be great
            }
        }
    }

    public void SetBlockOnTile(Block block)
    {
        tileOver.SetBlockHolding(block.GetComponent<Block>());
        List<GridTile> tileNeighbors = tileOver.GetTileNeighbors(prefabBlockPlacing.GetNeighbors());
        foreach (GridTile tile in tileNeighbors)
        {
            tile.SetBlockHolding(block.GetComponent<Block>());
        }
    }
}
