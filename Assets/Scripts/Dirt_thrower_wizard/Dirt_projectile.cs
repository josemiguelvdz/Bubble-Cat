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
    float rotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isRotated = false;
        Debug.Log("Start proyectil del lagarto " + dir);
    }

    void Update()
    {
        Debug.Log("Velocidad: " + dir);
        //Se calcula el vector que indica la velocidad a la que se mueve el proyectil
        if (isRotated == true)
        {
            //rb.SetRotation(rotation);   
            rb.velocity = dir.normalized * speed * Time.deltaTime;
        }
        else
        {
            rb.velocity = dir.normalized * speed * Time.deltaTime;
        }

    }

    public void setVelocity(Vector3 distance)
    {
        dir = distance;
        Debug.Log("Distance: " + dir);
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
        rot = rot - 90;
        Debug.Log("Rotacion es: " + rot);
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
        else if (rot > -90 && rot <= 0)
        {
            if (dir.x < 0)
                dir.x = -dir.x;
            if (dir.y > 0)
                dir.y = -dir.y;
        }
    }
}
