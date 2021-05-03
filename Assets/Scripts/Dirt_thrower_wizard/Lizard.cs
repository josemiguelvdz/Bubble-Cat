using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizard : MonoBehaviour
{
    //Obtenemos elementos necesarios
    //Obtiene el pide derecho, encargado de crear el RayCast encargado de detectar el suelo
    public Transform foot;
    //Obtiene el transform spawner, desde donde se van a instanciar los proyectiles
    public Transform spawnerProjectile;
    //Obtiene el objeto projectile de la carpeta Prefabs
    public GameObject dirtProjectile;
    //Indica el tiempo que pasa entre un proyectil y el siguiente
    public float shootCadenceSecs;
    //Indica si el lagarto puede disparar
    public bool canShoot;
    //Indica si me puedo destruir al chocar con el proximo objeto
    public bool destructible;
    //Obtenemos variables necesarias para el recalculo contionuo de los diferentes elementos
    //Se indica el tiempo de retardo tras el primer invoke
    private float initialTime;
    //Indica la direction en la que se mueve el lagarto, empezamos con movimiento hacia la derecha
    private string direction = "right";
    //Indica la velocidad del descenso del lagarto
    private float dropVelocity = 5;
    //Indica la rotacion inicial, para perder dicha referencia al rotar al lagarto dentro de la pompa 
    private Quaternion initialTrans;

    private SpriteRenderer sprite;
    private int contador = 0;
    private int desapariciones = 0;

    private Transform player; //Posicion del jugador
    private float radius; //Distancia a la que veo al jugador

    Vector3 dir;

    RaycastHit2D hitStage;

    void Start()
    {
        //Obtenemos el sprite del lagarto para poder hacer uso del volteo (flip)
        sprite = GetComponent<SpriteRenderer>();
        initialTime = shootCadenceSecs;
        initialTrans = transform.rotation;
        dir = Vector2.zero;
    }

    private void Update()
    {
        if (String.IsNullOrEmpty(direction))
        {
            transform.Translate(Vector3.zero * Time.deltaTime);
            killLizard();
        }
        else
        {
            //Detectamos al jugador y trazamos un rayo hacia él
            if (player != null)
            {
                //Debug.Log("Detecto player");
                Shoot();
            }

            if (direction != "drop")//No tiene que caerse el lagarto
            {
                hitStage = Physics2D.Raycast(foot.position, Vector2.up, 0.5f);
                //Debug.DrawRay(foot.position, Vector2.up * 0.5f, Color.green);

                if (hitStage != null && hitStage.collider != null) //Detecta suelo
                {
                    if (direction == "right")
                    {
                        //Indicamos la traslacion (movimiento) del lagarto hacia la derecha y cambio del sprite (volteo)
                        transform.Translate(Vector3.right * Time.deltaTime);
                        sprite.flipX = false;
                    }
                    else if (direction == "left")
                    {
                        //Indicamos la traslacion (movimiento) del lagarto hacia la izquierda yu cambio del scripte (volteo)
                        transform.Translate(Vector3.left * Time.deltaTime);
                        sprite.flipX = true;
                    }
                }
                else //No detecta suelo
                {
                    //Cambiamos al direccion del lagarto y actualizamos la posicion del pie
                    foot.localPosition = new Vector3(-foot.localPosition.x, foot.localPosition.y, foot.localPosition.z);
                    if (direction == "right")
                    {
                        direction = "left";
                        //foot.position = new Vector3(-foot.position.x, foot.position.y, foot.position.z);
                    }
                    else if (direction == "left")
                    {
                        direction = "right";
                        //foot.position = new Vector3(foot.position.x, foot.position.y, foot.position.z);
                    }
                }
            }
            else//Tiene que caerse el lagarto
            {
                //Asignamos la rotacion al lagarto, para generar un movimineto de caida vertical, ya que al haberlo rotado el movimiento saldra con la rotacion de la manipulacion de la pompa.
                transform.rotation = initialTrans;
                //Indicamos la traslacion (movimiento) del lagarto en descenso
                transform.Translate(Vector3.down * dropVelocity * Time.deltaTime);
            }
        }
    }

    //Dispara bala en la direction dada
    public void Shoot()
    {
        Debug.Log("Intento disparar");
        if (Time.time > initialTime && canShoot == true)
        {
            Debug.Log("Disparo");
            initialTime = Time.time + shootCadenceSecs;
            Instantiate(dirtProjectile, spawnerProjectile.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            setDirectionDirtProjectile(player.position - spawnerProjectile.position);
        }
    }


    public Vector2 getDirectionDirtProjectile()
    {
        return dir;
    }

    public void setDirectionDirtProjectile(Vector3 vector)
    {
        dir = vector;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //En este caso obtenemos el id del layer Stage
        int stageLayer = LayerMask.NameToLayer("Stage");

        //Ha colisionado con un objetro con layer Stage
        if (collision.gameObject.layer == stageLayer)
        {
            if (destructible == true)//Puedo destruirme, ya que si he interactuado con la pompa
            {
                //Animacion del lagarto
                direction = String.Empty;
            }
        }
    }

    private void killLizard()
    {
        contador++;
        if (contador % 350 == 0)
        {
            if (sprite.enabled == false)
            {
                sprite.enabled = true;
            }
            else
            {
                sprite.enabled = false;
            }
            desapariciones++;
        }

        if (desapariciones == 8)
        {
            gameObject.SetActive(false);
        }
    }

    public void StopShooting(bool shoot, bool destruction)
    {
        canShoot = false;
        if (destruction == true)
        {
            direction = "drop";
        }
    }

    public void PositionPlayer(Transform t, float s)
    {
        player = t;
        radius = s;
    }
}
