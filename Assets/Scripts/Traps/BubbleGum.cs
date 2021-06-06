using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGum : MonoBehaviour
{
    //Float que indica cuanto tenemos que pulsar el espacio
    public float canGetOut;
    //Float que hace de contador
    float getOut;
    //Boleano que indica si estamos dentro del chicle
    bool inSide;

    //Sonido al impactar con el chicle
    public AudioClip gumSound;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //Si estamos dentro
        if (inSide)
        {
            //Si pulsamos el espacio repetidas veces para salir
            if (Input.GetButtonDown("Jump"))
            {
                //Vamos sumando 1 al contador
                getOut++;
            }

            //Cuando el contador llega o supera las veces que hay que pulsar el espacio
            if (getOut >= canGetOut)
            {
                //Activamos el playerController y destruimos el chicle
                GameManager.GetInstance().ActivatePlayerController();
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si lo que entra es el jugador
        if (collision.GetComponent<PlayerController>())
        {
            //Desactivamos el playerController
            collision.GetComponent<PlayerController>().enabled = false;
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //Activamos el sonido del cichle
            audioSource.PlayOneShot(gumSound);
            //Boleana a true que indica que estamos dentro
            inSide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Si lo que sale es el player cuando un enemigo nos empuje, directamente salimos fuera
        if (collision.GetComponent<PlayerController>())
        {
            GameManager.GetInstance().ActivatePlayerController();
            Destroy(this.gameObject);
        }
    }
}
