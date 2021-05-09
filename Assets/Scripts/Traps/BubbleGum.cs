using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGum : MonoBehaviour
{
    public float canGetOut;
    float getOut;
    bool inSide;

    private void Update()
    {
        if (inSide)
        {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            collision.GetComponent<PlayerController>().enabled = false;
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            inSide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            GameManager.GetInstance().ActivatePlayerController();
            Destroy(this.gameObject);
        }
    }
}
