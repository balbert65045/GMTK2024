using UnityEngine;

// NOTE: Ensure name of animation matches enum
public enum PlayerAnimationState
{
    PLAYER_IDLE,
    PLAYER_MOVE,
    PLAYER_STANDARD_JUMP,
    PLAYER_CONNECTED_JUMP,
    PLAYER_SHOOT,
    PLAYER_STUN,
    PLAYER_REFILL
}

// Example: Call ChangeAnimationState(PlayerAnimationState.PLAYER_JUMP); from PlayerMovement...
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    PlayerAnimationState currentState;
    PlayerMovement playerMovement;
    bool grounded = true;
    bool moving = false;

    private void Start()
    {
        ChangeAnimationState(PlayerAnimationState.PLAYER_IDLE);
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Stun()
    {
        animator.SetBool("Stunned", true);
    }

    public void UnStun()
    {
        animator.SetBool("Stunned", false);
    }

    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }

    public void Eat()
    {
        animator.SetTrigger("Eat");
    }

    public void ConnectedJump()
    {
        if (!grounded)
        {
            animator.SetTrigger("ConnectedJump");
        }
    }


    private void Update()
    {
        if (!playerMovement.touchingGround && !playerMovement.touchingWall)
        {
            if (grounded)
            {
                grounded = false;
                animator.SetBool("Grounded", false);
            }
        }
        else
        {
            if (!grounded)
            {
                grounded = true;
                animator.SetBool("Grounded", true);
            }
        }

        if(playerMovement.RB.velocity.magnitude > .4f)
        {
            if (!moving)
            {
                moving = true;
                animator.SetBool("Moving", true);
            }
        }
        else
        {
            if (moving)
            {
                moving = false;
                animator.SetBool("Moving", false);
            }
        }
    }

    public void ChangeAnimationState(PlayerAnimationState newState)
    {
        if (currentState == newState) return;

        Debug.Log("Change Animation State:" + newState.ToString());
        animator.Play(newState.ToString());

        currentState = newState;
    }
}
