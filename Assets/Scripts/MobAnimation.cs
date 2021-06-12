using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAnimation : MonoBehaviour
{
    public Animator animator;
    private string currentState;

    const string MOB_IDLE = "Idle";
    const string MOB_MOVE = "Move";
    const string MOB_ATTACK = "Attack";
    const string MOB_HURT = "Hurt";
    const string MOB_DIE = "Die";

    [SerializeField] public float attackDelay;
    [SerializeField] public float hurtDelay;
    [SerializeField] public float dieDelay;


    // Start is called before the first frame update
    void Start()
    {
        ChangeAnimationState(MOB_IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        
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
