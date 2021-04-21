using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetecWall : MonoBehaviour
{
    EnemyMovement changeDirection;
    Transform r;
    void Start()
    {
        changeDirection = GetComponentInParent<EnemyMovement>();
        r = GetComponentInParent<Transform>();
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            changeDirection.direction = -changeDirection.direction;
            //r.localScale = new Vector3(-r.localScale.x, r.localScale.y, r.localScale.z);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            changeDirection.direction = -changeDirection.direction;
            //r.localScale = new Vector3(-r.localScale.x, r.localScale.y, r.localScale.z);
            
        }
    }
}
