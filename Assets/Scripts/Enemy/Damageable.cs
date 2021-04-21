using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int lives;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<BulletMovement>())
        {
            lives--;

            if(lives == 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
