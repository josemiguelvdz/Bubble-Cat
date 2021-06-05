using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<ManHole>())
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
