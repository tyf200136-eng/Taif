using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the door trigger is the Player
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                // Verify if the score is high enough to win
                if (GameManager.instance.totalScore >= GameManager.instance.starsToWin)
                {
                    GameManager.instance.WinGame();
                }
                else
                {
                    Debug.Log($"Door is locked! You have {GameManager.instance.totalScore}/{GameManager.instance.starsToWin} stars.");
                }
            }
        }
    }
}