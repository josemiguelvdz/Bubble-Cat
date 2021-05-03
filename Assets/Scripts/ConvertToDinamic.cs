using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertToDinamic : MonoBehaviour
{
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(this);
    }
}
