using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodPoint : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineVirtualCamera zoomOutCam;
    [SerializeField] Cinemachine.CinemachineVirtualCamera camUp;
    [SerializeField] Cinemachine.CinemachineVirtualCamera camDown;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<PlayerMovement>() != null)
        {
            collision.transform.GetComponent<PlayerStateManager>().SetGodPoint(this);
            if (camDown == null) { return; }
            FindObjectOfType<Cinemachine.CinemachineBrain>().m_DefaultBlend.m_Time = 1f;
            if (camDown.Priority == 2)
            {
                camDown.Priority = 1;
                camUp.Priority = 2;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<PlayerMovement>() != null)
        {
            collision.transform.GetComponent<PlayerStateManager>().SetGodPoint(null);
        }
    }

    public void SwitchCam(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.BUILD_MODE:
                camUp.Priority = 1;
                zoomOutCam.Priority = 2;
                break;
            case GameMode.PLATFORM_MODE:
                camUp.Priority = 2;
                zoomOutCam.Priority = 1;
                break;
        }
    }
}
