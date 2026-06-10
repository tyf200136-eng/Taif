using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float normalSpeed = 5f;
    public float slowedSpeed = 2.5f;

    [Header("Start Point")]
    public Transform startPoint;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isSlowed = false;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (startPoint != null)
        {
            transform.position = startPoint.position;
        }
    }

    void Update()
    {
        if (isDead) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;

        if (movement.magnitude > 0)
        {
            AudioManager.instance.PlayWalkingSound();
        }
        else
        {
            AudioManager.instance.StopWalkingSound();
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        float currentSpeed = isSlowed ? slowedSpeed : normalSpeed;
        rb.linearVelocity = movement * currentSpeed;
    }

    public void SetSlowed(bool value)
    {
        isSlowed = value;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        rb.linearVelocity = Vector2.zero;

        AudioManager.instance.StopWalkingSound();
        AudioManager.instance.StopDangerSound();
        AudioManager.instance.PlayDeathSound();

        Debug.Log("Player died!");

        gameObject.SetActive(false);
    }
}