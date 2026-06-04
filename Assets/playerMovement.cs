using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float normalSpeed = 5f;
    public float slowedSpeed = 2.5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isSlowed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        float currentSpeed = isSlowed ? slowedSpeed : normalSpeed;
        rb.linearVelocity = movement * currentSpeed;
    }

    public void SetSlowed(bool value)
    {
        isSlowed = value;
    }

    public void Die()
    {
        Debug.Log("Player died!");

        gameObject.SetActive(false);

        // Optional later:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}