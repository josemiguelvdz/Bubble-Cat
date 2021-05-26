using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRay : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;
    Magic parent;
    Vector3 direction;
    bool isRotated;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        parent = FindObjectOfType<Magic>();
        direction = parent.getDirection();
        rb.velocity = direction.normalized * speed * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<BubbleController>())
            Destroy(this.gameObject);
    }

    public void rotationProjectil(float rot)
    {
        isRotated = true;
        if (rot > 0 && rot <= 90)
        {
            if (direction.x < 0)
                direction.x = -direction.x;
            if (direction.y < 0)
                direction.y = -direction.y;
        }
        else if (rot > 90 && rot <= 180)
        {
            if (direction.x > 0)
                direction.x = -direction.x;
            if (direction.y < 0)
                direction.y = -direction.y;
        }
        else if (rot > 180 && rot <= 270)
        {
            if (direction.x > 0)
                direction.x = -direction.x;
            if (direction.y > 0)
                direction.y = -direction.y;
        }
        else if (rot > 270 && rot <= 360)
        {
            if (direction.x < 0)
                direction.x = -direction.x;
            if (direction.y > 0)
                direction.y = -direction.y;
        }

        rb.velocity = direction.normalized * speed * Time.fixedDeltaTime;
    }
}
