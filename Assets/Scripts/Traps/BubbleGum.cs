using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGum : MonoBehaviour
{
    public float canGetOut;
    float getOut;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            collision.GetComponent<PlayerController>().enabled = false;
            collision.transform.position = new Vector2(transform.position.x, transform.position.y + collision.GetComponent<CapsuleCollider2D>().size.y/2);

            if (Input.GetButtonDown("Jump"))
            {
                getOut++;
            }

            if (getOut >= canGetOut)
            {
                GameManager.GetInstance().ActivatePlayerController();
                Destroy(this.gameObject);
                
            }
        }
    }
}
