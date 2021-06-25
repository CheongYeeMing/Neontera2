using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossPathfindingAI : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] public bool passiveAggressive; // Will only chase when is attacked
    [SerializeField] public bool instantAggressive; // Chase once target enters radius

    protected Path path;
    protected Rigidbody2D rb;
    protected Seeker seeker;

    protected Vector2 direction;

    protected bool reachedEndOfPath;
    protected bool isChasingTarget;

    protected float speed;
    protected float nextWayPointDistance = 3f;

    protected int currentWaypoint = 0;

    // Start is called before the first frame update
    public virtual void Start()
    {
        isChasingTarget = false;
        if (instantAggressive)
        {
            isChasingTarget = true;
        }
        speed = GetComponent<BossMovement>().moveSpeed;
        seeker = GetComponent<Seeker>();
        //rb = gameObject.GetComponent<BossMovement>().GetRigidbody();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    public void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
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

        if (Mathf.Sign(GetComponent<BossMovement>().moveSpeed) != Mathf.Sign(direction.x) && Mathf.Abs(direction.x) > 0.1)
        {/*
            Debug.Log("Why u flip again cb");
            Debug.Log("Movespeed: " + gameObject.GetComponent<BossMovement>().moveSpeed);
            Debug.Log("Movespeed Sign: " + Mathf.Sign(gameObject.GetComponent<BossMovement>().moveSpeed));
            Debug.Log("Direction X: " + direction.x);
            Debug.Log("Direction X Sign: " + Mathf.Sign(direction.x));*/
            GetComponent<BossMovement>().Flip();
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }
    }

    public virtual void Update()
    {
        if (!isChasingTarget) return;

        if (!gameObject.GetComponent<BossHealth>().IsHurting() && !gameObject.GetComponent<BossHealth>().IsDead())
        {
            gameObject.GetComponent<BossMovement>().ChaseTarget(direction);
        }
    }

    public bool GetIsChasingTarget()
    {
        return isChasingTarget;
    }

    public void SetIsChasingTarget(bool isChasingTarget)
    {
        this.isChasingTarget = isChasingTarget;
    }
}
