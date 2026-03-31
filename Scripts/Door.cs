using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private GameObject door;
    private PolygonCollider2D doorCollider;
    private SpriteRenderer doorSpriteRenderer;
    private AudioSource doorAudioSource; // AudioSource to play sound
    [SerializeField] private AudioClip doorSound; // The sound to play when the door is interacted with

    void Start()
    {
        door = gameObject;
        doorCollider = GetComponent<PolygonCollider2D>();
        doorSpriteRenderer = GetComponent<SpriteRenderer>();
        doorAudioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (Manager.hasKey)
            {
                MakeDoorTransparent();
                SetColliderToTrigger();
                PlayDoorSound(); // Play the sound if the player has the key
                Debug.Log("Door is now passable (semi-transparent and trigger set)!");
            }
            else
            {
                Debug.Log("You need a key to open this door.");
            }
        }
    }

    private void MakeDoorTransparent()
    {
        if (doorSpriteRenderer != null)
        {
            Color color = doorSpriteRenderer.color;
            color.a = 0.5f;
            doorSpriteRenderer.color = color;
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on door!");
        }
    }

    private void SetColliderToTrigger()
    {
        if (doorCollider != null)
        {
            doorCollider.isTrigger = true;
        }
        else
        {
            Debug.LogError("PolygonCollider2D not found on door!");
        }
    }

    private void PlayDoorSound()
    {
        if (doorAudioSource != null && doorSound != null)
        {
            doorAudioSource.PlayOneShot(doorSound); // Play the sound
        }
        else
        {
            Debug.LogError("AudioSource or doorSound not set!");
        }
    }
}