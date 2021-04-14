using UnityEngine;

public class Proyectile : MonoBehaviour
{
    public float speed;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(transform.right.x, transform.right.y) * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<BubbleController>())
            Destroy(this.gameObject);
    }
}
