using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingMovement : BossMovement
{
    // Slime King Animation States
    protected const string BOSS_ENRAGED_IDLE = "EnragedIdle";
    protected const string BOSS_ENRAGED_MOVE = "EnragedMove";

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
        Patrol();
        if (gameObject.GetComponent<SlimeKingPathfindingAI>().instantAggressive && InChaseRange())
        {
            gameObject.GetComponent<SlimeKingPathfindingAI>().SetIsChasingTarget(true);
            return;
        }

        if (gameObject.GetComponent<SlimeKingHealth>().IsDead())
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
    }

    public override void Patrol()
    {
        gameObject.GetComponent<SlimeKingAnimation>().ChangeAnimationState(GetMoveState());
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
        if (gameObject.GetComponent<SlimeKingHealth>().IsHurting())
        {
            can = false;
        }
        else if (gameObject.GetComponent<SlimeKingHealth>().IsDead())
        {
            can = false;
        }
        else if (gameObject.GetComponent<SlimeKingAttack>().IsAttacking())
        {
            can = false;
        }
        else if (gameObject.GetComponent<SlimeKingHealth>().IsEnraging())
        {
            can = false;
        }
        return can;
    }

    public override void ChaseTarget(Vector2 direction)
    {
        if (!CanMove()) return;
        gameObject.GetComponent<SlimeKingAnimation>().ChangeAnimationState(GetMoveState());
        if (!InChaseRange() || player.gameObject.GetComponent<CharacterHealth>().IsDead())
        {
            gameObject.GetComponent<SlimeKingPathfindingAI>().SetIsChasingTarget(false);
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
        if (gameObject.GetComponent<SlimeKingHealth>().IsEnraged())
        {
            return BOSS_ENRAGED_IDLE;
        }
        else
        {
            return BOSS_IDLE;
        }
    }
    
    public string GetMoveState()
    {
        if (gameObject.GetComponent<SlimeKingHealth>().IsEnraged())
        {
            return BOSS_ENRAGED_MOVE;
        }
        else
        {
            return BOSS_MOVE;
        }
    }
}
