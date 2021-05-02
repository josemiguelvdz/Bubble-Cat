using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour
{
    public float gasSpeed;
    public float time;

    private void Start()
    {
        Invoke("DeleteGas", time);
    }

    void Update()
    {
        transform.Translate(gasSpeed * Vector2.down*Time.deltaTime);
    }

    void DeleteGas()
    {
        Destroy(this.gameObject);
    }
}
