using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt_projectile : MonoBehaviour
{
    //Indica la velocidad a la que se mueve la bala
    public float speed;
    Rigidbody2D rb;
    Vector3 dir;
    bool detection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dir = Vector3.down;
    }

    void FixedUpdate()
    {
        //Se calcula el vector que indica la velocidad a la que se mueve el proyectil
        //rb.velocity = Vector2.down * speed;
        if (detection == true)
        {
            Debug.Log("Translate");
            rb.transform.Translate(dir * speed * Time.deltaTime);
        }
        else
        {
            rb.transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si colisiona se destruye
        if (!collision.gameObject.GetComponent<BubbleController>())
            Destroy(this.gameObject);
    }   

    public void updatePosition(Vector2 vector)
    {
        detection = true;
        dir = new Vector3(vector.x, vector.y, 0);
        Debug.Log("Cambia direccion proyectil");
    }
}
