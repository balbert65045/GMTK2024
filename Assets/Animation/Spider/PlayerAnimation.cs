using UnityEngine;

// NOTE: Ensure name of animation matches enum
public enum PlayerAnimationState
{
    PLAYER_JUMP,
    PLAYER_IDLE
}

// Example: Call ChangeAnimationState(PlayerAnimationState.PLAYER_JUMP); from PlayerMovement...
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    PlayerAnimationState currentState;

    private void Start()
    {
        ChangeAnimationState(PlayerAnimationState.PLAYER_IDLE);
    }

    public void ChangeAnimationState(PlayerAnimationState newState)
    {
        if (currentState == newState) return;

        Debug.Log("Change Animation State:" + newState.ToString());
        animator.Play(newState.ToString());

        currentState = newState;
    }
}
