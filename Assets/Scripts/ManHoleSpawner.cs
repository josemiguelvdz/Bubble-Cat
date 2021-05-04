using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManHoleSpawner : MonoBehaviour
{
    public float cooldown, time;
    public GameObject ManHole;

    void Start()
    {
        InvokeRepeating("SpawnManHole", time, cooldown);
    }

    void SpawnManHole()
    {
        Instantiate(ManHole, transform.position, Quaternion.identity);
    }
}
