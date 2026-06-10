using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource backgroundSource;
    public AudioSource sfxSource;
    public AudioSource walkingSource;
    public AudioSource dangerSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip walkingSound;
    public AudioClip dangerSound;
    public AudioClip deathSound;
    public AudioClip starCollectSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundSource != null && backgroundMusic != null)
        {
            backgroundSource.clip = backgroundMusic;
            backgroundSource.loop = true;
            backgroundSource.Play();
        }
    }

    public void PlayWalkingSound()
    {
        if (walkingSource != null && walkingSound != null && !walkingSource.isPlaying)
        {
            walkingSource.clip = walkingSound;
            walkingSource.loop = true;
            walkingSource.Play();
        }
    }

    public void StopWalkingSound()
    {
        if (walkingSource != null && walkingSource.isPlaying)
        {
            walkingSource.Stop();
        }
    }

    public void PlayDangerSound()
    {
        if (dangerSource != null && dangerSound != null && !dangerSource.isPlaying)
        {
            dangerSource.clip = dangerSound;
            dangerSource.loop = true;
            dangerSource.Play();
        }
    }

    public void StopDangerSound()
    {
        if (dangerSource != null && dangerSource.isPlaying)
        {
            dangerSource.Stop();
        }
    }

    public void PlayDeathSound()
    {
        if (sfxSource != null && deathSound != null)
        {
            sfxSource.PlayOneShot(deathSound);
        }
    }

    public void PlayStarCollectSound()
    {
        if (sfxSource != null && starCollectSound != null)
        {
            sfxSource.PlayOneShot(starCollectSound);
        }
    }
}