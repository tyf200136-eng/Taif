using UnityEngine;

public class EnemyKillPlayer2D : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement2D player = collision.gameObject.GetComponent<PlayerMovement2D>();

            if (player != null)
            {
                player.Die();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement2D player = collision.GetComponent<PlayerMovement2D>();

            if (player != null)
            {
                player.Die();
            }
        }
    }
}