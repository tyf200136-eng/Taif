using UnityEngine;

public class EnemyRandomMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float waitTime = 1f;

    [Header("Square Area Settings")]
    public Vector2 areaCenter = Vector2.zero;
    public Vector2 areaSize = new Vector2(5f, 5f);

    private Vector2 targetPosition;
    private float waitCounter;
    private bool isWaiting;

    void Start()
    {
        PickNewTarget();
    }

    void Update()
    {
        if (isWaiting)
        {
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                isWaiting = false;
                PickNewTarget();
            }

            return;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
        {
            isWaiting = true;
            waitCounter = waitTime;
        }
    }

    void PickNewTarget()
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
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}