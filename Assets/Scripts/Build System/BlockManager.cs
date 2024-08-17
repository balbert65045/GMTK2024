using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _blocks;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _blocks[0].GetComponent<Block>().SelectBlock();
            Debug.Log("Block 1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _blocks[1].GetComponent<Block>().SelectBlock();
            Debug.Log("Block 2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _blocks[2].GetComponent<Block>().SelectBlock();
            Debug.Log("Block 3");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _blocks[3].GetComponent<Block>().SelectBlock();
            Debug.Log("Block 4");
        }
    }
}
