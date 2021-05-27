﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int lives;
    public float timeToExplodeTheBubble;
    Animator animator;
    public AudioClip killSound;
    AudioSource audioSource;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
                if (this.gameObject.GetComponent<Lizard>())
                {
                    //animator.SetBool("Death", true);
                    this.gameObject.GetComponent<Lizard>().isDestructible();
                    animator.SetBool("Mov", false);
                    animator.SetBool("Death", true);
                }
                if (this.gameObject.GetComponent<BatBehaviour>())
                {
                    animator.SetBool("Death", true);
                    GetComponent<SlimeBehaviour>().enabled = false;
                    Destroy(GetComponent<Damageable>());
                    this.gameObject.GetComponent<BatBehaviour>().die();
                }
                Destroy(this.gameObject, 1);
                audioSource.PlayOneShot(killSound);
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
            audioSource.PlayOneShot(killSound);

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
                if(this.gameObject.GetComponent<Lizard>())
                {
                    //animator.SetBool("Death", true);
                    this.gameObject.GetComponent<Lizard>().isDestructible();
                }
                if (this.gameObject.GetComponent<BatBehaviour>())
                {
                    animator.SetBool("Death", true);
                    GetComponent<SlimeBehaviour>().enabled = false;
                    Destroy(GetComponent<Damageable>());
                    this.gameObject.GetComponent<BatBehaviour>().die();
                }
                Destroy(this.gameObject, 1);
                Destroy(this.gameObject, 1);
                audioSource.PlayOneShot(killSound);
            }
        }
    }
}
