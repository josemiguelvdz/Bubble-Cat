using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public Vector2 force;
    private Vector2 opuesto;
    public Transform dad;

    private void Start()
    {
        opuesto = new Vector2(-force.x, force.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //No lo aplica a las pastillas de jabon
        if(!collision.GetComponent<Bar>())
        {
            if(dad.localScale.x<0) collision.gameObject.GetComponent<Rigidbody2D>().AddForce(opuesto, ForceMode2D.Impulse);
            else if(dad.localScale.x>0) collision.gameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }
    }
}
