using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPoint : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineVirtualCamera winCam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>() != null)
        {
            Win(collision.GetComponent<PlayerMovement>());
        }
    }

    public void Win(PlayerMovement pm)
    {
        GameManager.Instance.SetWon(true);
        pm._moveInput = Vector2.zero;
        pm.RB.velocity = Vector2.zero;
        winCam.Priority = 3;
        AudioManager.Instance.PlayWin();
        StartCoroutine("ShowWin");
    }

    IEnumerator ShowWin()
    {
        yield return new WaitForSeconds(1.2f);
        FindObjectOfType<WinScreenManager>().ShowWinScreen();
    }
}
