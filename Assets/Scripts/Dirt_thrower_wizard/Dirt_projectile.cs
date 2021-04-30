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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        parent = FindObjectOfType<Lizard>();
        dir = parent.DirectionDirtProjectile();
    }

    void FixedUpdate()
    {
        //Se calcula el vector que indica la velocidad a la que se mueve el proyectil
        if (dir == Vector2.zero)
            rb.velocity = Vector2.down;
        else
            rb.velocity = dir;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si colisiona se destruye
        if (!collision.gameObject.GetComponent<BubbleController>())
            Destroy(this.gameObject);
    }   
}
