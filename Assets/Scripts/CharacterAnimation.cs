using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour, Animation
{
    public Animator animator;
    
    private string currentState;

    // Charater Animation States
    private const string CHARACTER_IDLE = "Idle";
    private const string CHARACTER_RUN = "Run";
    private const string CHARACTER_JUMP = "Jump";
    private const string CHARACTER_ATTACK = "Attack";
    private const string CHARACTER_SPECIAL_ATTACK = "AttackFireball";
    private const string CHARACTER_HURT = "Hurt";
    private const string CHARACTER_DIE = "Die";

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
