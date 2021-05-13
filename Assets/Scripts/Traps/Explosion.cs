using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("a");
        if (collision.gameObject.GetComponent<BubbleHelmet>())
            collision.gameObject.GetComponent<BubbleHelmet>().MakeDamage();

        else if (collision.gameObject.GetComponent<EnemyHealth>())
            Destroy(collision.gameObject);
    }
}
