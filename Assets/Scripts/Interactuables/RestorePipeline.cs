using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorePipeline : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Pipeline>())
        {
            Destroy(collision.gameObject);
            transform.GetChild(0).gameObject.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(this);
        }
    }
}
