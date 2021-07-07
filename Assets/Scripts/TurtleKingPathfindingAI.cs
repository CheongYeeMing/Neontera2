using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TurtleKingPathfindingAI : BossPathfindingAI
{
    // Start is called before the first frame update
    public override void Start()
    {
        isChasingTarget = false;
        if (instantAggressive)
        {
            isChasingTarget = true;
        }
        speed = GetComponent<TurtleKingMovement>().moveSpeed;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    public override void FixedUpdate()
    {
        if (!isChasingTarget) return;

        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position);

        if (Mathf.Sign(GetComponent<TurtleKingMovement>().moveSpeed) != Mathf.Sign(direction.x) && Mathf.Abs(direction.x) > 0.1)
        {
            GetComponent<TurtleKingMovement>().Flip();
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!isChasingTarget) return;

        if (!GetComponent<TurtleKingHealth>().IsHurting() && !GetComponent<TurtleKingHealth>().IsDead())
        {
            GetComponent<TurtleKingMovement>().ChaseTarget(direction);
        }
    }
}
