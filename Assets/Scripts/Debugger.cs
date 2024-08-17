using UnityEngine;

public class Debugger : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            WebResourceController.Instance.IncrementWebCount(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            WebResourceController.Instance.DecrementWebCount(1);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance.ToggleGameMode();
        }
    }
}
