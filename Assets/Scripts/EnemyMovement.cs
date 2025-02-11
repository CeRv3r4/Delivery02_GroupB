using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyState { Patrolling, Chasing, Returning }
    private EnemyState state;

    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float chaseSpeed = 3.5f;
    public float detectionRange = 5f;
    public Transform player;

    private Vector3 target;
    private Vector3 lastKnownPlayerPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = EnemyState.Patrolling;
        target = pointB.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EnemyState.Patrolling:
                Patrol();
                DetectPlayer();
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Returning:
                ReturnToPatrol();
                break;
        }
    }

    private void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, pointA.position) < 0.1)
        {
            Flip();
            target = pointB.position;
        }
        else if (Vector3.Distance(transform.position, pointB.position) < 0.1)
        {
            Flip();
            target = pointA.position;
        }
    }

    private void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            lastKnownPlayerPosition = player.position;
            state = EnemyState.Chasing;
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            state = EnemyState.Returning;
        }
    }

    private void ReturnToPatrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, lastKnownPlayerPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, lastKnownPlayerPosition) < 0.1f)
        {
            state = EnemyState.Patrolling;
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
