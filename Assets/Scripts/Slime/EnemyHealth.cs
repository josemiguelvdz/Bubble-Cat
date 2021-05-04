﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int lives;
    public float timeToExplodeTheBubble;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Damageable>() && !collision.gameObject.GetComponent<EnemyHealth>())
        {
            lives--;

            if(lives == 0)
            {
                Destroy(this.gameObject);
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.gameObject.GetComponent<Trap>())
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Melee>())
        {
            lives--;
            Debug.Log(lives);
            if (lives == 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
