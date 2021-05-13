﻿using System.Collections;
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRotated == true)
        {
            //rb.SetRotation(rotation);   
            rb.velocity = direction.normalized * speed * Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity = direction.normalized * speed * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<BubbleController>())
            Destroy(this.gameObject);
    }

    public void rotationProjectil(float rot)
    {
        isRotated = true;
        rot = rot - 180;
        Debug.Log("Rotacion es: " + rot);
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
        else if (rot > -90 && rot <= 0)
        {
            if (direction.x < 0)
                direction.x = -direction.x;
            if (direction.y > 0)
                direction.y = -direction.y;
        }
    }
}
