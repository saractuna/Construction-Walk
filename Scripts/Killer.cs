using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    public int timer = 1;

    void Start()
    {
        Destroy(gameObject, (timer) * 1f);
    }
}