using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAnimation : MonoBehaviour, Animation
{
    public Animator animator;
    
    private string currentState;

    public const string MOB_IDLE = "Idle";
    public const string MOB_MOVE = "Move";
    public const string MOB_ATTACK = "Attack";
    public const string MOB_HURT = "Hurt";
    public const string MOB_DIE = "Die";

    // Start is called before the first frame update
    public void Start()
    {
        ChangeAnimationState(MOB_IDLE);
    }

    public void ChangeAnimationState(string newState)
    {
        // Stop the same animation from interupting itself
        if (currentState == newState) return;

        // Play the Animation
        animator.Play(newState);

        // Reassign current state with new state
        currentState = newState;
    }
}
