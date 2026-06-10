using UnityEngine;

public class EnemyLightDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement2D player = collision.GetComponent<PlayerMovement2D>();

            if (player != null)
            {
                player.SetSlowed(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement2D player = collision.GetComponent<PlayerMovement2D>();

            if (player != null)
            {
                player.SetSlowed(false);
            }
        }
    }
}