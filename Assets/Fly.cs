using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    [SerializeField] int WebAmount = 20;
    public int GetWebAmount() { return WebAmount; }
    bool isEaten = false;
    public void SetIsEatenTrue() {
        GetComponent<BugAnimationController>().Webbed();
        isEaten = true;
    }
    public bool GetIsEaten() { return isEaten; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEaten) { return; }
        if (collision.GetComponent<PlayerMovement>() != null)
        {
            collision.GetComponent<PlayerStateManager>().Stun(this.transform.position);
        }
    }
}
