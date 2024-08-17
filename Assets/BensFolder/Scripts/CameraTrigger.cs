using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineVirtualCamera camUp;
    [SerializeField] Cinemachine.CinemachineVirtualCamera camDown;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>() != null)
        {
            if (collision.transform.position.y > transform.position.y)
            {
                if (camDown.Priority == 2)
                {
                    FindObjectOfType<Cinemachine.CinemachineBrain>().m_DefaultBlend.m_Time = 1f;
                    camDown.Priority = 1;
                    camUp.Priority = 2;
                }
            }
            else if (collision.transform.position.y < transform.position.y)
            {
                if (camUp.Priority == 2)
                {
                    FindObjectOfType<Cinemachine.CinemachineBrain>().m_DefaultBlend.m_Time = .4f;
                    camUp.Priority = 1;
                    camDown.Priority = 2;
                }
            }
        }
    }
}
