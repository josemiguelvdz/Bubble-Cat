using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basura : MonoBehaviour
{
    bool OnBubble=false;
    public float   forceImpulse;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<BubbleController>()) Destroy(this.gameObject);
        else OnBubble = true;
        
    }


    private void OnEnable()
    {
        if (OnBubble)
        {
            Debug.Log(transform.up);
            rb.AddForce(new Vector2(transform.up.x * forceImpulse, transform.up.y * forceImpulse), ForceMode2D.Impulse);
            Debug.Log(transform.rotation);
        }
    }
}
