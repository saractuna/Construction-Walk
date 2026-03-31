using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 5;
    private Manager manager;

    private AudioSource coinAudioSource;
    [SerializeField] private AudioClip coinSound;  // Reference to the sound clip

    private void Start()
    {
        manager = FindObjectOfType<Manager>();
        coinAudioSource = GetComponent<AudioSource>();  // Get the AudioSource component
        if (coinAudioSource == null)
        {
            Debug.LogError("No AudioSource component found on Coin object!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            manager.AddCoin(coinValue);
            //PlayCoinSound();  // Play the sound when the coin is picked up
            Destroy(gameObject);  // Destroy the coin after the sound is played
        }
    }

    private void PlayCoinSound()
    {
        if (coinAudioSource != null && coinSound != null)
        {
            //Debug.Log("Playing Coin Sound");
            coinAudioSource.PlayOneShot(coinSound);
        }
        else
        {
            Debug.LogError("AudioSource or CoinSound not set!");
        }
    }
}