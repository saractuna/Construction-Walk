using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    SpriteRenderer sr;

    [SerializeField] Sprite activeCheckpoint;
    [SerializeField] Sprite inactiveCheckpoint;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            MakeActive();
        }
    }

    public void MakeActive()
    {
        sr.sprite = activeCheckpoint;
        Manager.Instance.UpdateCheckpoints(gameObject);
    }
    public void MakeInactive()
    {
        sr.sprite = inactiveCheckpoint;
    }
}
