using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cannon;
    public GameObject bullet;
    public float cadenceTime;
    public float cooldownTime;
    void Start()
    {
        InvokeRepeating("Shoot", cadenceTime, cooldownTime);
    }

    void Shoot()
    {
        Instantiate(bullet, cannon.transform.position, cannon.rotation);

    }


}
