using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteSoldierMovement : MobMovement
{
    protected const string MOB_JUMP = "Jump";

    public override void Patrol()
    {
        gameObject.GetComponent<MobAnimation>().ChangeAnimationState(MOB_MOVE);
        if (onFloatingPlatform)
        {
            if (isAtEdge)
            {
                Flip();
            }
        }
        else if (jumpBoxCollider.IsTouchingLayers(wallLayer) || (!insidePatrolRange()) || jumpBoxCollider.IsTouchingLayers(mobLayer))
        {
            Flip();
        }

        if (isGrounded() && hitStep())
        {
            gameObject.GetComponent<MobAnimation>().ChangeAnimationState(MOB_JUMP);
            rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.velocity.y + jumpPower);
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
    }

    public override void ChaseTarget(Vector2 direction)
    {
        if (!CanMove()) return;
        gameObject.GetComponent<MobAnimation>().ChangeAnimationState(MOB_MOVE);
        if (!InChaseRange() || player.gameObject.GetComponent<CharacterHealth>().IsDead())
        {
            gameObject.GetComponent<MobPathfindingAI>().SetIsChasingTarget(false);
            return;
        }

        if (isGrounded() && hitStep())
        {
            gameObject.GetComponent<MobAnimation>().ChangeAnimationState(MOB_JUMP);
            rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.velocity.y + jumpPower);
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
    }
}
