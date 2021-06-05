using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarMovement : MonoBehaviour
{
    public Vector2 force;

    private Rigidbody2D rb;

    private bool limitAchieved = false;

    private Vector3 initialPos;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPos = gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (!limitAchieved)
        {
            rb.AddForce(force * Time.deltaTime, ForceMode2D.Impulse);
        }
        else
        {
            if (Vector2.zero.magnitude == Mathf.RoundToInt(rb.velocity.y))
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                rb.AddForce(-force * Time.deltaTime, ForceMode2D.Impulse);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Limit")
        {
            limitAchieved = true;
            Debug.Log("Debería parar");
        }

        if(col.tag == "Enemy")
        {
            Destroy(col.gameObject);
        }
    }

    private void OnDisable()
    {
        gameObject.transform.position = initialPos;
        limitAchieved = false;
    }
}
