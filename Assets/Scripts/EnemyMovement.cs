using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyState { Patrolling, Chasing, Returning }
    private EnemyState state;

    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float chaseSpeed = 3.5f;
    public float detectionRange = 5f;
    public float visionAngle = 60f;
    public Transform player;

    private Vector3 target;
    private Vector3 lastKnownPlayerPosition;

    void Start()
    {
        state = EnemyState.Patrolling;
        target = pointB.position;
    }

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

        if (Vector3.Distance(transform.position, target) < 0.05f) 
        {
            if (target == pointA.position)
            {
                target = pointB.position;
            }
            else
            {
                target = pointA.position;
            }

            Flip();
        }
    }

    private void DetectPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized; 
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            float angleToPlayer = Vector3.Angle(transform.right, directionToPlayer); 

            if (angleToPlayer < visionAngle / 2)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange);

                if (hit.collider != null && hit.collider.transform == player)
                {
                    lastKnownPlayerPosition = player.position;
                    state = EnemyState.Chasing;
                }

            }
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SaveScoreAndLoadEnding();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SaveScoreAndLoadEnding();
        }
    }

    void OnDrawGizmosSelected()
    {
        Handles.color = new Color(1, 0, 0, 0.2f); 

        Vector3 forward = transform.right;
        Handles.DrawSolidArc(transform.position, Vector3.forward, Quaternion.Euler(0, 0, -visionAngle / 2) * forward, visionAngle, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
