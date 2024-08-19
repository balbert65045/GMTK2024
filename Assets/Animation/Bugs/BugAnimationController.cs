using UnityEngine;

public class BugAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;

    //private void Start()
    //{
    //    ChangeAnimationState(PlayerAnimationState.PLAYER_IDLE);
    //    playerMovement = GetComponent<PlayerMovement>();
    //}

    public void Webbed()
    {
        animator.SetTrigger("Webbed");
    }
}
