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
        this.rb = GetComponent<Rigidbody2D>();
        this.parent = FindObjectOfType<Lizard>();
        this.dir = parent.getDirectionDirtProjectile();
        this.isRotated = false;
    }

    void FixedUpdate()
    {
        //Se calcula el vector que indica la velocidad a la que se mueve el proyectil
        if (this.isRotated == true)
        {
            //rb.SetRotation(rotation);   
            this.rb.velocity = this.dir.normalized * this.speed * Time.fixedDeltaTime;
        }
        else
        {
            this.rb.velocity = this.dir.normalized * this.speed * Time.fixedDeltaTime;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si colisiona se destruye
        if (!collision.gameObject.GetComponent<BubbleController>())
        {
            Destroy(this.gameObject);
            if (this.isRotated == true)
            {
                this.isRotated = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collision2D collision)
    {
        // Si colisiona se destruye
        if (!collision.gameObject.GetComponent<BubbleController>())
        {
            Destroy(this.gameObject);           
        }
    }

    public void rotationProjectil(float rot)
    {
        this.isRotated = true;
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
}
