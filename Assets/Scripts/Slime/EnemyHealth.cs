using System.Collections;
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
        if (collision.gameObject.GetComponent<BulletMovement>())
        KillEnemy();
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
        KillEnemy();
    }

    public void KillEnemy()
    {
        lives--;
        Debug.Log(lives);
        if (lives <= 0)
        {
            if (gameObject.GetComponent<SlimeBehaviour>())
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);

                animator.SetBool("Death", true);
                GetComponent<SlimeBehaviour>().enabled = false;
                Destroy(GetComponent<Damageable>());

                Destroy(gameObject, 1);
            }
            if (gameObject.GetComponent<Lizard>())
            {
                //animator.SetBool("Death", true);
                gameObject.GetComponent<Lizard>().isDestructible();
                gameObject.transform.GetChild(0).gameObject.SetActive(true);

                Destroy(gameObject.GetComponent<Damageable>());

                Destroy(gameObject, 1);
            }
            if (gameObject.GetComponent<BatBehaviour>())
            {
                animator.SetBool("Death", true);
                gameObject.transform.GetChild(1).gameObject.SetActive(true);

                Destroy(GetComponent<Damageable>());
                this.gameObject.GetComponent<BatBehaviour>().die();
            }
            if (gameObject.GetComponent<SpiderMovement>())
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);

                Destroy(gameObject.GetComponent<Damageable>());
                Destroy(gameObject.GetComponent<SpiderMovement>());
                Destroy(gameObject.GetComponent<Bubbleable>());

                Destroy(gameObject, 1);
            }

            audioSource.PlayOneShot(killSound);
        }

    }
}
