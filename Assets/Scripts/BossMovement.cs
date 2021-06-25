using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public Transform edgeDetector;
    [SerializeField] public Collider2D boxCollider;
    [SerializeField] public Collider2D jumpBoxCollider;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public LayerMask wallLayer;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float patrolRadius;
    [SerializeField] public float chaseRadius;
    [SerializeField] public float jumpPower;
    [SerializeField] public bool onFloatingPlatform;

    protected Rigidbody2D rb;

    protected bool isPatrolling;
    protected bool isAtEdge;
    protected bool inPatrolRange;

    protected Vector2 spawnPoint;

    // Mob Animation States
    protected const string BOSS_IDLE = "Idle";
    protected const string BOSS_MOVE = "Move";

    // Start is called before the first frame update
    public virtual void Start()
    {
        isPatrolling = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        spawnPoint = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!CanMove())
        {
            StopPatrol();
            return;
        }
        Patrol();
        if (gameObject.GetComponent<BossPathfindingAI>().instantAggressive && InChaseRange())
        {
            gameObject.GetComponent<BossPathfindingAI>().SetIsChasingTarget(true);
            return;
        }

        if (gameObject.GetComponent<BossHealth>().IsDead())
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

    public void FixedUpdate()
    {
        if (isPatrolling && onFloatingPlatform)
        //this uses the groundCheckPosition to check if the platform ends
        {
            isAtEdge = !Physics2D.OverlapCircle(edgeDetector.position, 0.1f, groundLayer);
        }
    }

    public virtual void Patrol()
    {
        gameObject.GetComponent<BossAnimation>().ChangeAnimationState(BOSS_MOVE);
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

    public void StopPatrol()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public virtual bool CanMove()
    {
        bool can = true;
        if (gameObject.GetComponent<BossHealth>().IsHurting())
        {
            can = false;
        }
        else if (gameObject.GetComponent<BossHealth>().IsDead())
        {
            can = false;
        }
        else if (gameObject.GetComponent<BossAttack>().IsAttacking())
        {
            can = false;
        }
        return can;
    }

    public bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.03f, groundLayer);
        return raycastHit.collider != null;
    }

    public bool hitStep()
    {
        return jumpBoxCollider.IsTouchingLayers(groundLayer);
    }

    public bool insidePatrolRange()
    {
        if (inPatrolRange)
        {
            return true;
        }
        else
        {
            if (transform.position.x < spawnPoint.x && moveSpeed > 0)
            {
                return true;
            }
            else if (transform.position.x > spawnPoint.x && moveSpeed < 0)
            {
                return true;
            }
            return false;
        }
    }

    public void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        moveSpeed *= -1;
        isAtEdge = false;
    }

    public virtual void ChaseTarget(Vector2 direction)
    {
        gameObject.GetComponent<BossAnimation>().ChangeAnimationState(BOSS_MOVE);
        if (!InChaseRange() || player.gameObject.GetComponent<CharacterHealth>().IsDead())
        {
            gameObject.GetComponent<BossPathfindingAI>().SetIsChasingTarget(false);
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

    public bool InChaseRange()
    {
        return Vector2.Distance(spawnPoint, player.position) < chaseRadius;
    }

    public Rigidbody2D GetRigidbody()
    {
        return rb;
    }

    public Vector2 GetSpawnPoint()
    {
        return spawnPoint;
    }
}
