using UnityEngine;

public class StarCollectible : MonoBehaviour
{
    public int starValue = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerScore playerScore = collision.GetComponent<PlayerScore>();

            if (playerScore != null)
            {
                playerScore.AddScore(starValue);
            }

            AudioManager.instance.PlayStarCollectSound();

            Destroy(gameObject);
        }
    }
}