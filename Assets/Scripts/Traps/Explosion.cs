using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BubbleHelmet>())
            collision.gameObject.GetComponent<BubbleHelmet>().MakeDamage();

        else if (collision.gameObject.GetComponent<EnemyHealth>())
            Destroy(collision.gameObject);

        else if(collision.gameObject.GetComponent<BrokenWall>())
            Destroy(collision.gameObject);
    }
}
