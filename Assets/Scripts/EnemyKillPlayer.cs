using UnityEngine;

public class EnemyKillPlayer2D : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detect player via Collision
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. Play player death effects/sound (if Player script exists)
            PlayerMovement2D player = collision.gameObject.GetComponent<PlayerMovement2D>();
            if (player != null)
            {
                player.Die();
            }

            // 2. Open the Game Over Screen and freeze the game
            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detect player via Trigger
        if (collision.CompareTag("Player"))
        {
            // 1. Play player death effects/sound (if Player script exists)
            PlayerMovement2D player = collision.GetComponent<PlayerMovement2D>();
            if (player != null)
            {
                player.Die();
            }

            // 2. Open the Game Over Screen and freeze the game
            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
        }
    }
}