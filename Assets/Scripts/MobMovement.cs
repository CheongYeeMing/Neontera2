using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMovement : MonoBehaviour
{
    public float moveSpeed;
    public float distFromPlayer;

    public bool isPatrolling;
    public bool isAtEdge;
    public bool inPatrolRange;

    public Rigidbody2D rb;

    public Collider2D boxCollider;
    public Collider2D jumpBoxCollider;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    
    public Transform player;
    public Transform edgeDetector;

    public Vector2 spawnPoint;

    [SerializeField] public float patrolRadius;
    [SerializeField] public float jumpPower;
    [SerializeField] public bool onFloatingPlatform;

    // Mob Animation States
    public const string MOB_IDLE = "Idle";
    public const string MOB_MOVE = "Move";

    // Start is called before the first frame update
    void Start()
    {
        isPatrolling = true;
        spawnPoint = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPatrolling)
        {
            if (!gameObject.GetComponent<MobHealth>().isHurting && !gameObject.GetComponent<MobHealth>().isDead && !gameObject.GetComponent<MobPathfindingAI>().isChasingTarget)
            {
                gameObject.GetComponent<MobAnimation>().ChangeAnimationState(MOB_MOVE);
                Patrol();
            }
        } 
        
        if (!gameObject.GetComponent<MobHealth>().isHurting && !gameObject.GetComponent<MobHealth>().isDead && gameObject.GetComponent<MobPathfindingAI>().isChasingTarget)
        {
            gameObject.GetComponent<MobAnimation>().ChangeAnimationState(MOB_MOVE);
        }

        if (gameObject.GetComponent<MobHealth>().isDead)
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

    void FixedUpdate()
    {
        if (isPatrolling && onFloatingPlatform)
        //this uses the groundCheckPosition to check if the platform ends
        {
            isAtEdge = !Physics2D.OverlapCircle(edgeDetector.position, 0.1f, groundLayer);
        }
    }

    void Patrol()
    {
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

    private bool isGrounded()
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
    //function to flip the enemy
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        moveSpeed *= -1;
        isAtEdge = false;
    }

    public void ChaseTarget(Vector2 direction)
    {
        if (Mathf.Sign(moveSpeed) != Mathf.Sign(direction.x))
        {
            Flip();
        }

        if ((!insidePatrolRange()))
        {
            gameObject.GetComponent<MobPathfindingAI>().isChasingTarget = false;
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
}
