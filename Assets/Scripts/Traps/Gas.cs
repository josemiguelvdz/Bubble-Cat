using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour
{
    public float gasSpeed;
    //float que indica el tiempo que tarda el gas en destruirse
    public float time;

    private void Start()
    {
        Invoke("DeleteGas", time);
    }

    void Update()
    {
        //Movemos el gas hacia abajo
        transform.Translate(gasSpeed * Vector2.down*Time.deltaTime);
    }

    void DeleteGas()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si choca con la pompa o con algún ente con damageable
        if (collision.GetComponent<BubbleController>() || collision.GetComponent<Damageable>())
        {
            //Destrimos el gas
            Destroy(collision.gameObject);
        }
    }
}
