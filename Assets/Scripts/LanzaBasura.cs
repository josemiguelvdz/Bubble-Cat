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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Basura>()) collision.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }
    void Shoot()
    {
        Instantiate<GameObject>(GarbagePrefab,transform.position,Quaternion.identity);
    }


}
