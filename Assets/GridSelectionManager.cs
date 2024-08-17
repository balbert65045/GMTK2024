using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSelectionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.Mode == GameMode.BUILD_MODE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Select Cube
                //Physics2D.Ra
            }
        }
    }
}
