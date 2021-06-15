using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour, Animation
{
    public Animator animator;
    
    private string currentState;

    // Charater Animation States
    public const string CHARACTER_IDLE = "Idle";
    public const string CHARACTER_RUN = "Run";
    public const string CHARACTER_JUMP = "Jump";
    public const string CHARACTER_ATTACK = "Attack";
    public const string CHARACTER_SPECIAL_ATTACK = "AttackFireball";
    public const string CHARACTER_HURT = "Hurt";
    public const string CHARACTER_DIE = "Die";

    public void Start()
    {
        ChangeAnimationState(CHARACTER_IDLE);
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
