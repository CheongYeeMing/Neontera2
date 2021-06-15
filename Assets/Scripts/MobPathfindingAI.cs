using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MobPathfindingAI : MonoBehaviour
{
    [SerializeField] public bool passiveAggressive; // Will only chase when is attacked
    [SerializeField] public bool instantAggressive; // Chase once target enters radius

    public Transform target;

    public float speed;
    public float nextWayPointDistance = 3f;

    private Path path;
    public int currentWaypoint = 0;
    public bool reachedEndOfPath;

    public Rigidbody2D rb;
    private Seeker seeker;

    public Vector2 direction;

    public bool isChasingTarget;

    // Start is called before the first frame update
    void Start()
    {
        isChasingTarget = false;
        if (instantAggressive)
        {
            isChasingTarget = true;
        }
        speed = gameObject.GetComponent<MobMovement>().moveSpeed;
        seeker = gameObject.GetComponent<Seeker>();
        rb = gameObject.GetComponent<MobMovement>().rb;
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
    void FixedUpdate()
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

        if (Mathf.Sign(gameObject.GetComponent<MobMovement>().moveSpeed) != Mathf.Sign(direction.x) && Mathf.Abs(direction.x) > 0.1)
        {
            Debug.Log("Why u flip again cb");
            Debug.Log("Movespeed: " + gameObject.GetComponent<MobMovement>().moveSpeed);
            Debug.Log("Movespeed Sign: " + Mathf.Sign(gameObject.GetComponent<MobMovement>().moveSpeed));
            Debug.Log("Direction X: " + direction.x);
            Debug.Log("Direction X Sign: " + Mathf.Sign(direction.x));
            gameObject.GetComponent<MobMovement>().Flip();
        }

        

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }
    }

    public void Update()
    {
        if (!isChasingTarget) return;

        if (!gameObject.GetComponent<MobHealth>().isHurting && !gameObject.GetComponent<MobHealth>().isDead)
        {
            gameObject.GetComponent<MobMovement>().ChaseTarget(direction);
        }
    }
}
