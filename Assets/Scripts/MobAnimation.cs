using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAnimation : MonoBehaviour, Animation
{
    [SerializeField] public Animator animator;
    
    protected string currentState;

    protected const string MOB_IDLE = "Idle";
    protected const string MOB_MOVE = "Move";
    protected const string MOB_ATTACK = "Attack";
    protected const string MOB_HURT = "Hurt";
    protected const string MOB_DIE = "Die";

    // Start is called before the first frame update
    public virtual void Start()
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
