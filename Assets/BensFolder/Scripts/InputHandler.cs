using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InputHandler : MonoBehaviour
{
    PlayerMovement playerMovement;
    WebShooter webShooter;
    [SerializeField] GridVisualizer gridVisualizer;
    [SerializeField] InventoryController inventoryController;


    // Start is called before the first frame update
    void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        webShooter = FindObjectOfType<WebShooter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && playerMovement.GetComponent<PlayerStateManager>().currentGodPointTouching != null) {
            Move(Vector2.zero);
            GameManager.Instance.ToggleGameMode();
            if (GameManager.Instance.Mode == GameMode.BUILD_MODE)
            {
                FindObjectOfType<PlayerStateManager>().currentGridOn.gameObject.SetActive(true);
                inventoryController.gameObject.SetActive(true);
            }
            else
            {
                FindObjectOfType<PlayerStateManager>().currentGridOn.gameObject.SetActive(false);
                inventoryController.gameObject.SetActive(false);

            }
            playerMovement.GetComponent<PlayerStateManager>().currentGodPointTouching.SwitchCam(GameManager.Instance.Mode);
        }

        if (GameManager.Instance.Mode != GameMode.PLATFORM_MODE) { return; }
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            JumpCut();
        }

        if(Input.GetMouseButtonDown(0)) {
            ShootWeb();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ReleaseWeb();
        }
        Move(new Vector2(x, y));
    }

    void ShootWeb()
    {
        webShooter.FireWeb();

    }

    void ReleaseWeb()
    {
        webShooter.ReleaseWeb();
    }


    void JumpCut()
    {
        playerMovement.JumpRelease();
    }

    void Jump()
    {
        playerMovement.Jump();
    }

    void Move(Vector2 moveInput)
    {
        playerMovement.Move(moveInput);
        webShooter.Swing(moveInput);
    }
}
