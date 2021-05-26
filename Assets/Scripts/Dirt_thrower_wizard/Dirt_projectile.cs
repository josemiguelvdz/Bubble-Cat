using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt_projectile : MonoBehaviour
{
    //Indica la velocidad a la que se mueve la bala
    public float speed = 200;
    Rigidbody2D rb;
    Vector3 dir;
    bool isRotated;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isRotated = false;
        Debug.Log("Start proyectil del lagarto " + dir);
        rb.velocity = dir.normalized * speed * Time.deltaTime;
    }

    void Update()
    {

    }

    public void setVelocity(Vector3 distance)
    {
        dir = distance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si colisiona se destruye
        if (!collision.gameObject.GetComponent<BubbleController>())
        {
            Destroy(gameObject);
            if (isRotated == true)
            {
                isRotated = false;
            }
        }
    }

    public void rotationProjectil(float rot)
    {
        isRotated = true;
        if (rot > 0 && rot <= 90)
        {
            if (dir.x < 0)
                dir.x = -dir.x;
            if (dir.y < 0)
                dir.y = -dir.y;
        }
        else if (rot > 90 && rot <= 180)
        {
            if (dir.x > 0)
                dir.x = -dir.x;
            if (dir.y < 0)
                dir.y = -dir.y;
        }
        else if (rot > 180 && rot <= 270)
        {
            if (dir.x > 0)
                dir.x = -dir.x;
            if (dir.y > 0)
                dir.y = -dir.y;
        }
        else if (rot > 270 && rot <= 360)
        {
            if (dir.x < 0)
                dir.x = -dir.x;
            if (dir.y > 0)
                dir.y = -dir.y;
        }

        rb.velocity = dir.normalized * speed * Time.deltaTime;
    }
}
