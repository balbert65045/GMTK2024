using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPieceManager : MonoBehaviour
{
    [SerializeField] GameObject Block1Prefab;
    [SerializeField] GameObject Block2Prefab;
    [SerializeField] GameObject Block3Prefab;

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
                FindObjectOfType<GridSelectionManager>().SetCurrentPlacingBlock(Block1Prefab);
                //GetComponent<BlockManager>()
            }
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                FindObjectOfType<GridSelectionManager>().SetCurrentPlacingBlock(Block2Prefab);
                //GetComponent<BlockManager>()
            }
            if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                FindObjectOfType<GridSelectionManager>().SetCurrentPlacingBlock(Block3Prefab);
                //GetComponent<BlockManager>()
            }
        }
    }
}
