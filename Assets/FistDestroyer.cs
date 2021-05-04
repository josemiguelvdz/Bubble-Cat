using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistDestroyer : MonoBehaviour
{
    public float timeDestroyer;

    private void Start()
    {
        Destroy(this.gameObject, timeDestroyer);
    }
}
