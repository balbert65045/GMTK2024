using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPieceManager : MonoBehaviour
{
    [SerializeField] GameObject Block1Prefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.Mode == GameMode.BUILD_MODE)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameObject block = Instantiate(Block1Prefab);
                block.GetComponent<Block>().SelectBlock();
                block.GetComponent<Block>().FindSelectedTile();
                //GetComponent<BlockManager>()
            }
        }
    }
}
