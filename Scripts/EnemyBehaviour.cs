using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    SpriteRenderer sr;

    [SerializeField] int initialLives = 3;
    int currentLives;

    //Colours
    Color originalSpriteColor;
    [SerializeField] Color deathColor;
    
    void Start()
    {
        deathColor = Color.red;
        sr = GetComponent<SpriteRenderer>();
        currentLives = initialLives;
        originalSpriteColor = sr.color;
    }

    public void TakeDamage()
    {
        currentLives--;

        if (currentLives <= 0)
        {
            Destroy(gameObject);
        }

        float percentageDamageTaken = 1f - ((float) currentLives / initialLives);
        Color newColor = Color.Lerp(originalSpriteColor, deathColor, percentageDamageTaken);
        sr.color = newColor;
    }
}
