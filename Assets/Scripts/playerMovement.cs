using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float normalSpeed = 5f;
    public float slowedSpeed = 1.5f;
    public Transform startPoint;

    [Header("Sprites")] // هذه هي الخانات التي ستبحثين عنها
    public Sprite frontSprite; 
    public Sprite backSprite;  
    public Sprite sideSprite;  

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 movement;
    private bool isDead = false;
    private bool isSlowed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (startPoint != null)
            transform.position = startPoint.position;
    }

    void Update()
    {
        if (isDead) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // --- نظام تبديل الصور حسب الاتجاه ---
        if (movement.x != 0) // يمين أو يسار
        {
            sr.sprite = sideSprite; 
            sr.flipX = (movement.x < 0); // يقلب الصورة إذا مشى يسار
        }
        else if (movement.y > 0) // فوق (ظهر الروبوت)
        {
            sr.sprite = backSprite;
            sr.flipX = false;
        }
        else if (movement.y < 0) // تحت (وجه الروبوت)
        {
            sr.sprite = frontSprite;
            sr.flipX = false;
        }

        movement = movement.normalized;
        HandleAudio();
    }

    void FixedUpdate()
    {
        if (isDead) return;
        float currentSpeed = isSlowed ? slowedSpeed : normalSpeed;
        rb.linearVelocity = movement * currentSpeed;
    }

    void HandleAudio()
    {
        if (AudioManager.instance == null) return;
        if (movement.magnitude > 0) AudioManager.instance.PlayWalkingSound();
        else AudioManager.instance.StopWalkingSound();
    }

    public void SetSlowed(bool value) => isSlowed = value;

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        if (AudioManager.instance != null) {
            AudioManager.instance.StopWalkingSound();
            AudioManager.instance.PlayDeathSound();
        }
        gameObject.SetActive(false);
    }
}