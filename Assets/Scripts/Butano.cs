using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butano : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2 velocityLimit;
    private void OnCollisionEnter2D(Collision2D col)
    {

        if (Mathf.Abs(col.relativeVelocity.x) > velocityLimit.x)
        {
            Explode();
        }
        if(Mathf.Abs(col.relativeVelocity.y) > velocityLimit.y)
        {
            Explode();
        }

    }

    void Explode()
    {
        // PUM particulas y tal
        Destroy(this.gameObject);
    }
}
