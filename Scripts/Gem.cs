using UnityEngine;

public class GemCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object colliding is the Player
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player collided with the Gem!");

            // Call Manager to activate the win screen
            Manager.Instance.ActivateWinScreen();

            // Destroy the Gem object
            Destroy(gameObject);
        }
    }
}