using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManHole : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        rb.velocity = Vector2.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<BubbleController>())
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        rb.velocity = Vector2.right * speed;
    }
}
