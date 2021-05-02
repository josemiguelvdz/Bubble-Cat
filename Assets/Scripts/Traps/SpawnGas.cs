using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGas : MonoBehaviour
{
    public GameObject gas;
    public float cooldown;
    float time = 1;

    void Start()
    {
        InvokeRepeating("ReleaseGas", time, cooldown);
    }

    void ReleaseGas()
    {
        Instantiate(gas, transform.position, transform.rotation);
    }
}
