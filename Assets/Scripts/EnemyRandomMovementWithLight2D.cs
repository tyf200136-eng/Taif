using UnityEngine;

public class SmartEnemyMovement2D : MonoBehaviour
{
    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public float waitTime = 1f;

    [Header("Detection")]
    public Transform player;
    public float detectionRange = 5f;
    public float stopChasingRange = 7f;

    [Header("Patrol Area")]
    public Vector2 areaCenter = Vector2.zero;
    public Vector2 areaSize = new Vector2(8f, 8f);

    [Header("Avoid")]
    public LayerMask blockedLayers; // SafeZone + Wall
    public float checkRadius = 0.4f;

    [Header("Light")]
    public Transform enemyLight;
    public float lightTurnSpeed = 8f;

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private Vector2 moveDirection;
    private float waitCounter;
    private bool isWaiting;
    private bool isChasing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        PickNewTarget();
    }

    void FixedUpdate()
    {
        CheckPlayerDistance();

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            PatrolRandomly();
        }

        RotateLightSmoothly();
    }

    void CheckPlayerDistance()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(rb.position, player.position);

        if (!isChasing && distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            isWaiting = false;
        }
        else if (isChasing && distanceToPlayer >= stopChasingRange)
        {
            isChasing = false;
            PickNewTarget();
        }
    }

    void ChasePlayer()
    {
        if (player == null) return;

        Vector2 playerPosition = player.position;
        Vector2 currentPosition = rb.position;

        moveDirection = (playerPosition - currentPosition).normalized;

        Vector2 nextPosition = currentPosition + moveDirection * chaseSpeed * Time.fixedDeltaTime;

        bool blocked = Physics2D.OverlapCircle(nextPosition, checkRadius, blockedLayers);

        if (!blocked)
        {
            rb.MovePosition(nextPosition);
        }
        else
        {
            isChasing = false;
            PickNewTarget();
        }
    }

    void PatrolRandomly()
    {
        if (isWaiting)
        {
            waitCounter -= Time.fixedDeltaTime;

            if (waitCounter <= 0)
            {
                isWaiting = false;
                PickNewTarget();
            }

            return;
        }

        Vector2 currentPosition = rb.position;
        moveDirection = (targetPosition - currentPosition).normalized;

        Vector2 nextPosition = Vector2.MoveTowards(
            currentPosition,
            targetPosition,
            patrolSpeed * Time.fixedDeltaTime
        );

        bool blocked = Physics2D.OverlapCircle(nextPosition, checkRadius, blockedLayers);

        if (!blocked)
        {
            rb.MovePosition(nextPosition);
        }
        else
        {
            PickNewTarget();
            return;
        }

        if (Vector2.Distance(rb.position, targetPosition) < 0.15f)
        {
            isWaiting = true;
            waitCounter = waitTime;
        }
    }

    void PickNewTarget()
    {
        int attempts = 0;

        do
        {
            float randomX = Random.Range(
                areaCenter.x - areaSize.x / 2f,
                areaCenter.x + areaSize.x / 2f
            );

            float randomY = Random.Range(
                areaCenter.y - areaSize.y / 2f,
                areaCenter.y + areaSize.y / 2f
            );

            targetPosition = new Vector2(randomX, randomY);
            attempts++;

        } while (Physics2D.OverlapCircle(targetPosition, checkRadius, blockedLayers) && attempts < 50);
    }

    void RotateLightSmoothly()
    {
        if (enemyLight == null) return;
        if (moveDirection.sqrMagnitude < 0.01f) return;

        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        enemyLight.rotation = Quaternion.Lerp(
            enemyLight.rotation,
            targetRotation,
            lightTurnSpeed * Time.fixedDeltaTime
        );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & blockedLayers) != 0)
        {
            isChasing = false;
            PickNewTarget();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(areaCenter, areaSize);

        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.DrawWireSphere(transform.position, stopChasingRange);

        Gizmos.DrawWireSphere(targetPosition, checkRadius);
    }
}