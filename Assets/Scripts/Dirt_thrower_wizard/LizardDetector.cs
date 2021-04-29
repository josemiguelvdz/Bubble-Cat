using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardDetector : MonoBehaviour
{
    Lizard parent;
    CircleCollider2D col;

    private void Start()
    {
        parent = GetComponentInParent<Lizard>();
        col = GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            parent.PositionPlayer(collision.gameObject.transform, col.radius);
        }
    }
}
