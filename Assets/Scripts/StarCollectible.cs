using UnityEngine;

public class StarCollectible : MonoBehaviour
{
    public int starValue = 1;

    [Header("Effects")]
    [Tooltip("Optional: Particle effect prefab to play when collected.")]
    public GameObject collectEffectPrefab;
    
    [Tooltip("Optional: Sound clip to play if AudioManager.instance is missing.")]
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Notify GameManager to increase score and spawn a new star
            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(starValue);
            }

            // 2. Trigger audio and visual feedback
            PlayCollectEffects();

            // 3. Destroy this star
            Destroy(gameObject);
        }
    }

    private void PlayCollectEffects()
    {
        // Play sound using global AudioManager if it exists
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayStarCollectSound();
        }
        // Fallback: Play sound directly at position if clip is provided
        else if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        // Instantiate visual particle effect if provided
        if (collectEffectPrefab != null)
        {
            Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
        }
    }
}