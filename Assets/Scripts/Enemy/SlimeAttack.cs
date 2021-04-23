using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    SlimeBehaviour stop;

    void Start()
    {
        stop = GetComponentInParent<SlimeBehaviour>();
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
