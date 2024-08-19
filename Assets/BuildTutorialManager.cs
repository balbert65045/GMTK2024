using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTutorialManager : MonoBehaviour
{
    [SerializeField] GameObject StartTutorialCanvas;
    [SerializeField] GameObject BuildTutorialCanvas;
    [SerializeField] GameObject FirstTutorialCanvas;
    [SerializeField] GameObject SecondTutorialCanvas;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GameModeChangedEvent?.AddListener(GameModeChanged);
        InventoryController.Instance.SelectedBlockTypeChangedEvent?.AddListener(InventoryChanged);
    }

    void GameModeChanged(GameMode mode)
    {
        if (mode == GameMode.BUILD_MODE)
        {
            StartTutorialCanvas.SetActive(false);
            BuildTutorialCanvas.SetActive(true);
          
        }
        else
        {
            SecondTutorialCanvas.SetActive(false);
            //StartTutorialCanvas.SetActive(true);
        }
    }

    void InventoryChanged(BlockType _type)
    {
        Debug.Log("inventory changed");
        if (BuildTutorialCanvas.activeSelf)
        {
            FirstTutorialCanvas.SetActive(false);
            SecondTutorialCanvas.SetActive(true);
        }
    }
}
