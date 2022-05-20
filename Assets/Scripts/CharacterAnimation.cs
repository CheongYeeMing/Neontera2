using UnityEngine;

public class CharacterAnimation : MonoBehaviour, Animation
{
    // Charater Animation States
    private const string CHARACTER_IDLE = "Idle";
    private const string CHARACTER_RUN = "Run";
    private const string CHARACTER_JUMP = "Jump";
    private const string CHARACTER_ATTACK = "Attack";
    private const string CHARACTER_ATTACK_2 = "Attack2";
    private const string CHARACTER_ATTACK_3 = "Attack3";
    private const string CHARACTER_SPECIAL_ATTACK = "AttackFireball";
    private const string CHARACTER_HURT = "Hurt";
    private const string CHARACTER_DIE = "Die";
    private const string CHARACTER_WALLHANG = "WallHang";

    public Animator animator;
    private string currentState;


    public void Start()
    {
        ChangeAnimationState(CHARACTER_IDLE);
    }
    public void ChangeAnimationState(string newState)
    {
        // Stop the same animation from interupting itself
        if (currentState == newState)
        {
            if (currentState == CHARACTER_ATTACK || currentState == CHARACTER_ATTACK_2)
            {
                animator.StopPlayback();
            }
            else
            {
                //Debug.Log("return with anim state");
                //Debug.Log(currentState);
                //Debug.Log(newState);
                return;
            }
        }

        // Play the Animation
        animator.Play(newState);

        // Reassign current state with new state
        currentState = newState;
    }
}
