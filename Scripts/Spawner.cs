using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public float spawnTime = 1f;

    void Start()
    {
        InvokeRepeating("SpawnBalls", 0f, spawnTime);
    }

    void SpawnBalls()
    {
        Instantiate(ballPrefab, transform.position, Quaternion.identity);
    }
}