using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private AudioSource keyAudioSource;
    [SerializeField] private AudioClip keySound;

    void Start()
    {
        keyAudioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        if (keyAudioSource == null)
        {
            Debug.LogError("No AudioSource component found on Key object!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Manager.KeyPickup();
            Destroy(gameObject);
        }
    }
}