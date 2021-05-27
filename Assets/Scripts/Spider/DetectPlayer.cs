using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D cc;
    Vector3 spider;

    public float raycastDistance;

    RaycastHit2D  hit;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<Collider2D>();
        spider = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position != spider) rb.gravityScale = 1;



        hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - cc.bounds.size.y / 2), Vector2.down, raycastDistance);

        

        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.name);
            if (hit.transform.GetComponent<PlayerController>()) rb.bodyType = RigidbodyType2D.Dynamic;
         
        }
    }


    private void OnCollisionEnter2D(Collision2D collision) 
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Stage") || collision.gameObject.GetComponent<PlayerController>())
        {
            animator.SetBool("Cuelga", false);
            GetComponent<SpiderMovement>().enabled = true;
            Destroy(this);
        }
    }
}
