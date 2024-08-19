using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public GodPoint currentGodPointTouching;
    public GridVisualizer currentGridOn;
    public void SetGodPoint(GodPoint point) { currentGodPointTouching = point; }
    public void SetCurrentGridOn(GridVisualizer grid) { currentGridOn = grid; }

    public bool isStunned = false;
    [SerializeField] float StunDurration = 1f;
    [SerializeField] float InvincibleDurration = 1f;
    [SerializeField] float StunForce = 2f;

    public void Stun(Vector2 locationOfStunning)
    {
        if (!isStunned)
        {
            isStunned = true;
            PlayerMovement pm = GetComponent<PlayerMovement>();
            Vector2 dir = ((Vector2)transform.position - locationOfStunning).normalized;
            float x = dir.x > 0 ? 1 : -1;
            float y = 1;
            Vector2 forceDir = new Vector2 (x, y);
            pm.RB.velocity = Vector2.zero;
            pm.RB.AddForce(forceDir * StunForce, ForceMode2D.Impulse);
            pm.DisableMoveInputs();
            GetComponent<WebShooter>().SetWebShooting(false);
            GetComponent<PlayerAnimation>().Stun();
            StartCoroutine("WaitToStopBeingStunned");
        }
    }

    IEnumerator WaitToStopBeingStunned()
    {
        yield return new WaitForSeconds(StunDurration);
        PlayerMovement pm = GetComponent<PlayerMovement>();
        pm.EnableMoveInputs();
        GetComponent<WebShooter>().SetWebShooting(true);
        GetComponent<PlayerAnimation>().UnStun();
        yield return new WaitForSeconds(InvincibleDurration);
        isStunned = false;
    }

}
