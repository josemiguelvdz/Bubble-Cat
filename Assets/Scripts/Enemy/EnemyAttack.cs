using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    EnemyMovement stop;

    void Start()
    {
        stop = GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            Debug.Log("Ataco");
            stop.direction = Vector2.zero;
        }
    }
}
