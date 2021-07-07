using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleKingMovement : BossMovement
{
    // Start is called before the first frame update
    public override void Start()
    {
        isPatrolling = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        spawnPoint = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!CanMove()) return;
        
        if (gameObject.GetComponent<TurtleKingPathfindingAI>().instantAggressive && InChaseRange())
        {
            gameObject.GetComponent<TurtleKingPathfindingAI>().SetIsChasingTarget(true);
            return;
        }

        if (gameObject.GetComponent<TurtleKingHealth>().IsDead())
        {
            StopPatrol();
        }

        if (Vector2.Distance(spawnPoint, new Vector2(transform.position.x, transform.position.y)) > patrolRadius)
        {
            inPatrolRange = false;
        }
        else
        {
            inPatrolRange = true;
        }
        Patrol();
    }

    public override void Patrol()
    {
        gameObject.GetComponent<TurtleKingAnimation>().ChangeAnimationState(GetMoveState());
        if (onFloatingPlatform)
        {
            if (isAtEdge)
            {
                Flip();
            }
        }
        else if (jumpBoxCollider.IsTouchingLayers(wallLayer) || (!insidePatrolRange()))
        {
            Flip();
        }

        if (isGrounded() && hitStep())
        {
            rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.velocity.y + jumpPower);
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
    }

    public override bool CanMove()
    {
        bool can = true;
        if (gameObject.GetComponent<TurtleKingHealth>().IsHurting())
        {
            can = false;
        }
        else if (gameObject.GetComponent<TurtleKingHealth>().IsDead())
        {
            can = false;
        }
        else if (gameObject.GetComponent<TurtleKingAttack>().IsAttacking())
        {
            can = false;
        }
        return can;
    }

    public override void ChaseTarget(Vector2 direction)
    {
        if (!CanMove()) return;
        gameObject.GetComponent<TurtleKingAnimation>().ChangeAnimationState(GetMoveState());
        if (!InChaseRange() || player.gameObject.GetComponent<CharacterHealth>().IsDead())
        {
            gameObject.GetComponent<TurtleKingPathfindingAI>().SetIsChasingTarget(false);
            Patrol();
            return;
        }

        if (isGrounded() && hitStep())
        {
            rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.velocity.y + jumpPower);
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
    }

    public string GetIdleState()
    {
        return BOSS_IDLE;
    }

    public string GetMoveState()
    {
         return BOSS_MOVE;
    }
}
