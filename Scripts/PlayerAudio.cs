using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] walkConcreteSounds;
    [SerializeField] private AudioClip[] walkPlatformSounds;
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private AudioClip[] climbSounds;

    [Header("Interaction Sounds")]
    [SerializeField] private AudioClip checkpointSound;
    [SerializeField] private AudioClip interactableSound;

    [Header("Background Music")]
    [SerializeField] private AudioClip backgroundMusic;
    private AudioSource backgroundMusicSource;

    private string currentSurface = "Concrete";

    void Start()
    {
        // Initialize the AudioSource for sound effects
        audioSource = GetComponent<AudioSource>();

        // Initialize the AudioSource for background music
        GameObject bgMusicObject = new GameObject("BackgroundMusic");
        bgMusicObject.transform.parent = transform; // Attach to the player
        backgroundMusicSource = bgMusicObject.AddComponent<AudioSource>();
        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.volume = 0.5f; // Adjust volume as needed
        backgroundMusicSource.playOnAwake = true;
        backgroundMusicSource.Play();
    }

    // For walking sound
    public void PlayWalkSound()
    {
        if (audioSource.isPlaying) return;

        AudioClip walkClip = null;

        if (currentSurface == "Concrete" && walkConcreteSounds.Length > 0)
        {
            walkClip = GetRandomClip(walkConcreteSounds);
        }
        else if (currentSurface == "Platform" && walkPlatformSounds.Length > 0)
        {
            walkClip = GetRandomClip(walkPlatformSounds);
        }

        if (walkClip != null)
        {
            audioSource.PlayOneShot(walkClip);
        }
    }

    public void PlayJumpSound()
    {
        if (jumpSounds.Length > 0)
        {
            AudioClip jumpClip = GetRandomClip(jumpSounds);
            audioSource.PlayOneShot(jumpClip);
        }
    }

    public void PlayClimbSound()
    {
        if (climbSounds.Length > 0)
        {
            AudioClip climbClip = GetRandomClip(climbSounds);
            audioSource.PlayOneShot(climbClip);
        }
    }

    // Handle interactions with checkpoints and interactables
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            if (checkpointSound != null)
            {
                Debug.Log("Player collided with a Checkpoint.");
                audioSource.PlayOneShot(checkpointSound);
            }
        }
        else if (collision.CompareTag("Interactables"))
        {
            if (interactableSound != null)
            {
                Debug.Log("Player collided with an Interactable.");
                audioSource.PlayOneShot(interactableSound);
            }
        }
    }

    // Helper method to get a random clip from an array of clips
    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }

    public void SetCurrentSurface(string surface)
    {
        currentSurface = surface;
    }

    // Control background music
    public void StopBackgroundMusic()
    {
        if (backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
        }
    }

    public void PlayBackgroundMusic()
    {
        if (!backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Play();
        }
    }
}