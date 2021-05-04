using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRay : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;
    Magic parent;
    Vector3 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        parent = FindObjectOfType<Magic>();
        direction = parent.getDirection();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rb.velocity = Vector2.left * speed * Time.fixedDeltaTime;
        rb.velocity = direction.normalized * speed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<BubbleController>())
            Destroy(this.gameObject);
    }
}
