using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Si colisiona con la basura se destruye
        if (collision.gameObject.GetComponent<Basura>()) Destroy(this.gameObject);
    }
}
