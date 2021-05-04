using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzaBasura : MonoBehaviour
{
    public GameObject GarbagePrefab;
    public Vector2 force;
    public float timeToStartShooting, cadenceShooting;

    private void Start()
    {
        
        InvokeRepeating("Shoot", timeToStartShooting, cadenceShooting);
    }

    void Shoot()
    {
        GameObject trash = Instantiate<GameObject>(GarbagePrefab,transform.position,Quaternion.identity);
        trash.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }


}
