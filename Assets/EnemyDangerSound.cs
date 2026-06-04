using UnityEngine;

public class EnemyDangerSound : MonoBehaviour
{
    public Transform player;
    public float dangerDistance = 4f;

    private bool dangerSoundPlaying = false;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= dangerDistance)
        {
            if (!dangerSoundPlaying)
            {
                AudioManager.instance.PlayDangerSound();
                dangerSoundPlaying = true;
            }
        }
        else
        {
            if (dangerSoundPlaying)
            {
                AudioManager.instance.StopDangerSound();
                dangerSoundPlaying = false;
            }
        }
    }
}