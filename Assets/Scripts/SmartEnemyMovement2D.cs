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

    [Header("Sprites")]
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite sideSprite;

    [Header("Patrol Area")]
    public Vector2 areaCenter = Vector2.zero;
    public Vector2 areaSize = new Vector2(8f, 8f);

    [Header("Avoid")]
    public LayerMask blockedLayers;
    public float checkRadius = 0.4f;

    [Header("Light")]
    public Transform enemyLight;
    public float lightTurnSpeed = 8f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 targetPosition;
    private Vector2 moveDirection;
    private float waitCounter;
    private bool isWaiting;
    private bool isChasing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null) player = playerObject.transform;
        }
        PickNewTarget();
    }

    void FixedUpdate()
    {
        CheckPlayerDistance();

        if (isChasing) ChasePlayer();
        else PatrolRandomly();

        UpdateEnemySprite(); 
        RotateLightSmoothly();
    }

    void UpdateEnemySprite()
    {
        if (moveDirection.sqrMagnitude < 0.01f) return;

        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            sr.sprite = sideSprite;
            sr.flipX = (moveDirection.x < 0);
        }
        else
        {
            if (moveDirection.y > 0) sr.sprite = backSprite;
            else sr.sprite = frontSprite;
            sr.flipX = false;
        }
    }

    void CheckPlayerDistance()
    {
        if (player == null) return;
        float distanceToPlayer = Vector2.Distance(rb.position, player.position);
        if (!isChasing && distanceToPlayer <= detectionRange) { isChasing = true; isWaiting = false; }
        else if (isChasing && distanceToPlayer >= stopChasingRange) { isChasing = false; PickNewTarget(); }
    }

    void ChasePlayer()
    {
        if (player == null) return;
        moveDirection = ((Vector2)player.position - rb.position).normalized;
        Vector2 nextPosition = rb.position + moveDirection * chaseSpeed * Time.fixedDeltaTime;
        if (!Physics2D.OverlapCircle(nextPosition, checkRadius, blockedLayers)) rb.MovePosition(nextPosition);
        else { isChasing = false; PickNewTarget(); }
    }

    void PatrolRandomly()
    {
        if (isWaiting)
        {
            waitCounter -= Time.fixedDeltaTime;
            if (waitCounter <= 0) { isWaiting = false; PickNewTarget(); }
            moveDirection = Vector2.zero;
            return;
        }
        moveDirection = (targetPosition - rb.position).normalized;
        Vector2 nextPosition = Vector2.MoveTowards(rb.position, targetPosition, patrolSpeed * Time.fixedDeltaTime);
        if (!Physics2D.OverlapCircle(nextPosition, checkRadius, blockedLayers)) rb.MovePosition(nextPosition);
        else PickNewTarget();

        if (Vector2.Distance(rb.position, targetPosition) < 0.15f) { isWaiting = true; waitCounter = waitTime; }
    }

    void PickNewTarget()
    {
        int attempts = 0;
        do {
            float randomX = Random.Range(areaCenter.x - areaSize.x / 2f, areaCenter.x + areaSize.x / 2f);
            float randomY = Random.Range(areaCenter.y - areaSize.y / 2f, areaCenter.y + areaSize.y / 2f);
            targetPosition = new Vector2(randomX, randomY);
            attempts++;
        } while (Physics2D.OverlapCircle(targetPosition, checkRadius, blockedLayers) && attempts < 50);
    }

    void RotateLightSmoothly()
    {
        if (enemyLight == null || moveDirection.sqrMagnitude < 0.01f) return;
        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
        enemyLight.rotation = Quaternion.Lerp(enemyLight.rotation, Quaternion.Euler(0f, 0f, targetAngle), lightTurnSpeed * Time.fixedDeltaTime);
    }

    // --- Collision / Trigger Checking Logic ---

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Checks if the triggered object has the tag "Player"
        if (other.CompareTag("Player"))
        {
            PlayerMovement2D playerMovement = other.GetComponent<PlayerMovement2D>();
            if (playerMovement != null)
            {
                playerMovement.Die();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Checks if the collided object has the tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement2D playerMovement = collision.gameObject.GetComponent<PlayerMovement2D>();
            if (playerMovement != null)
            {
                playerMovement.Die();
            }
        }
    }
}