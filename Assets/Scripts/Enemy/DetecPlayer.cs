using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetecPlayer : MonoBehaviour
{
    SlimeBehaviour chasePlayer;
    public Transform player;

    void Start()
    {
        chasePlayer = GetComponentInParent<SlimeBehaviour>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            if (player.position.x < transform.position.x)
            {
                chasePlayer.direction = Vector2.left;
            }

            else
            {
                chasePlayer.direction = Vector2.right;
            }
        }
    }
}
