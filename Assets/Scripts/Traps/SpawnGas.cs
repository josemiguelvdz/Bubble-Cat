using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGas : MonoBehaviour
{
    public GameObject gas;
    //Float que indica el tiempo que hay entre gas y gas
    public float cooldown;
    float time = 1;

    void Start()
    {
        InvokeRepeating("ReleaseGas", time, cooldown);
    }

    void ReleaseGas()
    {
        //Instanciamos el gas
        Instantiate(gas, transform.position, transform.rotation);
    }
}
