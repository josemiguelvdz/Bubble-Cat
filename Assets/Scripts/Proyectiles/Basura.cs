using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basura : MonoBehaviour
{
    [Tooltip("Ipulso con el que se lanza la basura"), SerializeField]
    float forceImpulse;

    bool OnBubble=false;


    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Si no colisiona con la burbuja se destruye
        if (!collision.gameObject.GetComponent<BubbleController>()) Destroy(this.gameObject);
        else OnBubble = true;
        
    }
    private void OnEnable()
    {
        //Si se activa y estaba en una burbuja, se le añade la fuerza en la direccion correspondiente
        if (OnBubble)
        {
            Debug.Log(transform.up);
            rb.AddForce(new Vector2(transform.up.x * forceImpulse, transform.up.y * forceImpulse), ForceMode2D.Impulse);
            Debug.Log(transform.rotation);
        }
    }
}
