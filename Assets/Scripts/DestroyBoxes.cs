using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBoxes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            col.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 200), ForceMode2D.Impulse);

            
            Destroy(gameObject);
        }
        
    }
}
