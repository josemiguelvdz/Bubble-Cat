using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusMovement : MonoBehaviour

    // instrucciones: de por si esta dentro del collider
    // si sales del collider, la camra se pone en las coordenadas del player y comienza a seguirle
    // una vez te pares, el collider se pone sobre las coordenadas del player (y la camara tmb)
    // por tanto, la camara deja de seguir hasta que vuelvas a salir del collider
    // la camara está parada si estás dentro del collider
{
    public bool inside = true;

    public GameObject player;

    public Vector3 offset;

    Rigidbody2D pRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        pRigidBody = player.GetComponent<Rigidbody2D>();

        GameManager.GetInstance().SetInside(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!inside && pRigidBody.velocity == Vector2.zero)
        {
            transform.position = player.transform.position + (offset * player.transform.localScale.x);
        }

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            inside = false;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            inside = true;
        }
    }
}
