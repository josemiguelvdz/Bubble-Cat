using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt_projectile : MonoBehaviour
{
    //Indica la velocidad a la que se mueve la bala
    public float speed;
    Rigidbody2D rb;
    Vector2 dir;
    Lizard parent;
    bool isRotated;
    Quaternion rotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        parent = FindObjectOfType<Lizard>();
        dir = parent.getDirectionDirtProjectile();
        this.tag = "Enemy";
        this.gameObject.layer = 9;
    }

    void FixedUpdate()
    {
        //Se calcula el vector que indica la velocidad a la que se mueve el proyectil
        if (isRotated == true)
        {
            rb.MoveRotation(rotation);
        }
        else
        {
            if (dir == Vector2.zero)
                rb.velocity = Vector2.down;
            else
                rb.velocity = dir;
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

    public void rotationProjectil(Quaternion rot)
    {
        isRotated = true;
        rotation = rot;
    }
}
