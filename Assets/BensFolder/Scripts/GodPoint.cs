using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodPoint : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineVirtualCamera camUp;
    [SerializeField] Cinemachine.CinemachineVirtualCamera camDown;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<PlayerMovement>() != null)
        {
            FindObjectOfType<Cinemachine.CinemachineBrain>().m_DefaultBlend.m_Time = 1f;
            if (camDown.Priority == 2)
            {
                camDown.Priority = 1;
                camUp.Priority = 2;
            }
        }
    }
}
