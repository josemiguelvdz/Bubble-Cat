using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int lives;
    public float timeToExplodeTheBubble;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Damageable>() && !collision.gameObject.GetComponent<EnemyHealth>())
        {
            lives--;

            if (lives == 0)
            {
                if (this.gameObject.GetComponent<SlimeBehaviour>())
                {
                    animator.SetBool("Death", true);
                    GetComponent<SlimeBehaviour>().enabled = false;
                    Destroy(GetComponent<Damageable>());
                }
                Destroy(this.gameObject, 1);
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Trap>())
        {
            if (this.gameObject.GetComponent<SlimeBehaviour>())
            {
                animator.SetBool("Death", true);
                GetComponent<SlimeBehaviour>().enabled = false;
                Destroy(GetComponent<Damageable>());
            }
            Destroy(this.gameObject, 1);
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
                if (this.gameObject.GetComponent<SlimeBehaviour>())
                {
                    animator.SetBool("Death", true);
                    GetComponent<SlimeBehaviour>().enabled = false;
                    Destroy(GetComponent<Damageable>());
                }
                if (this.gameObject.GetComponent<BatBehaviour>())
                {
                    animator.SetBool("Death", true);
                }
                Destroy(this.gameObject, 1);
            }
        }
    }
}
