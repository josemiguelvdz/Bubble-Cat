using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt_projectile : MonoBehaviour
{
    //Indica la velocidad a la que se mueve la bala
    private float speed = 100;
    Rigidbody2D rb;
    Vector3 dir;
    Lizard parent;
    bool isRotated;
    float rotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        parent = FindObjectOfType<Lizard>();
        dir = parent.getDirectionDirtProjectile();
        isRotated = false;
    }

    void FixedUpdate()
    {
        //Se calcula el vector que indica la velocidad a la que se mueve el proyectil
        if (isRotated == true)
        {
            //rb.SetRotation(rotation);   
            rb.velocity = dir.normalized * speed * Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity = dir.normalized * speed * Time.fixedDeltaTime;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si colisiona se destruye
        if (!collision.gameObject.GetComponent<BubbleController>())
        {
            Destroy(this.gameObject);
            if (isRotated == true)
            {
                isRotated = false;
            }
        }
    }

    public void rotationProjectil(float rot)
    {
        isRotated = true;
        if (rot > 90 && rot <= 180)
        {
            if (dir.x < 0)
                dir.x = -dir.x;
            if (dir.y < 0)
                dir.y = -dir.y;
        }
        else if (rot > 180 && rot <= 270)
        {
            if (dir.x > 0)
                dir.x = -dir.x;
            if (dir.y < 0)
                dir.y = -dir.y;
        }
        else if (rot > 270 && rot <= 0)
        {
            if (dir.x > 0)
                dir.x = -dir.x;
            if (dir.y > 0)
                dir.y = -dir.y;
        }
        else if (rot > 0 && rot <= 90)
        {
            if (dir.x < 0)
                dir.x = -dir.x;
            if (dir.y > 0)
                dir.y = -dir.y;
        }
    }

    public void StopShooting(bool shoot, bool destruction)
    {
        parent.StopShooting(shoot, destruction);
    }
}
