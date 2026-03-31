using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    PlayerController playerController;
    Rigidbody2D rb;

    float enemyBounceForce;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        enemyBounceForce = playerController.enemyBounceForce;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Killzone")
        {
            Manager.Instance.ImmidiateRestart();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (playerController.falling && transform.position.y > collision.transform.position.y)
            {
                collision.gameObject.GetComponent<EnemyBehaviour>().TakeDamage();
                rb.velocity = new Vector2(rb.velocity.x, playerController.jumpForce / enemyBounceForce);
            }
            else
            {
                Manager.Instance.ImmidiateRestart();
            }
        }
    }
}
