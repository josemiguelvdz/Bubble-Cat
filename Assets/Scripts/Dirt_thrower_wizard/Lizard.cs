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
    public GameObject spawnerProjectile;
    //Obtiene el objeto projectile de la carpeta Prefabs
    private GameObject dirtProjectile;
    //Indica el tiempo que pasa entre un proyectil y el siguiente
    public float shootCadenceSecs;
    //Indica si queremos que haya una repeticion de projectiles o no
    public bool autoShoot;
    //Indica si el lagarto puede disparar
    public bool canShoot = false;
    //Indica si me puedo destruir al chocar con el proximo objeto
    public bool destructible;
    //Obtenemos variables necesarias para el recalculo contionuo de los diferentes elementos
    //Se indica el tiempo de retardo tras el primer invoke
    private float initialTime;
    //Indica la direction en la que se mueve el lagarto, empezamos con movimiento hacia la derecha
    private string direction = "right";
    //Indica la posicion del spawner del proyectil, como empezamos con movimiento hacia la derecha la poscion es una en concreto, si es movimiento hacia la izquierda es otra
    private Vector3 posSpawner = new Vector3(0.5f, -0.75f, 0);
    //Indica la posicion en donde se situa el inicio del proyectil, como empezamos con movimiento hacia la derecha la poscion es una en concreto, si es movimiento hacia la izquierda es otra
    private Vector3 posProjectile = new Vector3(0, -0.15f, 0);
    //Indica la posicion del pie delantero, como empezamos con movimiento hacia la derecha la poscion es una en concreto, si es movimiento hacia la izquierda es otra
    private Vector3 posFoot = new Vector3(0.15f, 0.25f, 0);
    //Indica la velocidad del descenso del lagarto
    private float velocity = 5;
    private Vector3 scaleChange = new Vector3(0.75f, 0.75f, 0);
    //Indica la rotacion inicial, para perder dicha referencia al rotar al lagarto dentro de la pompa 
    private Quaternion initialTrans;
   
    private SpriteRenderer sprite;
    private int contador = 0;
    private int desapariciones = 0;

    private Transform player; //Posicion del jugador
    private float radius; //Distancia a la que veo al jugador

    void Start()
    {
        //Obtenemos el sprite del lagarto para poder hacer uso del volteo (flip)
        sprite = GetComponent<SpriteRenderer>();
        initialTime = shootCadenceSecs;
        //Asignamos algunos valores iniciales que iran modificandose a lo largo de la ejecucion y es necesario tenerlas almacenadas        
        spawnerProjectile.transform.position = transform.position + posSpawner;
        dirtProjectile = spawnerProjectile.transform.GetChild(0).gameObject;
        dirtProjectile.transform.position = spawnerProjectile.transform.position + new Vector3(0, -0.15f, 0);
        dirtProjectile.transform.localScale = scaleChange;
        foot.transform.position = transform.position + posFoot;
        initialTrans = transform.rotation;

        if (autoShoot) //Esta activado el autoShoot se generan continuamente proyectiles
            InvokeRepeating("Shoot", initialTime, shootCadenceSecs);
        else //NO esta activado el autoShoot se generan un unico proyectil
            Invoke("Shoot", shootCadenceSecs);
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
            if (player != null && canShoot == false)
            {
                Vector2 dir = player.position - spawnerProjectile.transform.position;
                RaycastHit2D hitPlayer = Physics2D.Raycast(spawnerProjectile.transform.position, dir, radius);
                dirtProjectile.GetComponent<Dirt_projectile>().updatePosition(dir.normalized);
                Debug.DrawRay(spawnerProjectile.transform.position, dir.normalized * hitPlayer.distance, Color.red);
                canShoot = true;
            }

            if (direction != "drop")//No tiene que caerse el lagarto
            {
                RaycastHit2D hit = Physics2D.Raycast(foot.transform.position, Vector3.up, 0.05f);

                if (hit != null && hit.collider != null) //Detecta suelo
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
                    if (direction == "right")
                    {
                        direction = "left";
                        foot.transform.position = new Vector3((transform.position.x + -posFoot.x), (transform.position.y + posFoot.y), (transform.position.z + posFoot.z));
                    }
                    else if (direction == "left")
                    {
                        direction = "right";
                        foot.transform.position = transform.position + posFoot;
                    }
                }

                //Actualizamos la posicion tanto del spawner como del proyectil ya que el lagarto esta en movimiento continuo, y al instanciar el nuevo proyectil debe tener estas referencias
                if (direction == "right")
                {
                    spawnerProjectile.transform.position = transform.position + posSpawner;
                    dirtProjectile.transform.position = spawnerProjectile.transform.position + posProjectile;
                }
                else if (direction == "left")
                {
                    spawnerProjectile.transform.position = new Vector3((transform.position.x + -posSpawner.x), (transform.position.y + posSpawner.y), (transform.position.z + posSpawner.z));
                    dirtProjectile.transform.position = spawnerProjectile.transform.position + posProjectile;
                }
            }
            else//Tiene que caerse el lagarto
            {
                //Asignamos la rotacion al lagarto, para generar un movimineto de caida vertical, ya que al haberlo rotado el movimiento saldra con la rotacion de la manipulacion de la pompa.
                transform.rotation = initialTrans;
                //Indicamos la traslacion (movimiento) del lagarto en descenso
                transform.Translate(Vector3.down * velocity * Time.deltaTime);
            }
        }
    }

    //Dispara bala en la direction dada
    public void Shoot()
    {
        if (canShoot == true)
        {
            Instantiate(dirtProjectile, dirtProjectile.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
        canShoot = false;
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
            Destroy(gameObject);
        }
    }

    public void StopShooting()
    {
        canShoot = false;
        direction = "drop";
        destructible = true;
    }

    public void PositionPlayer(Transform t, float s) 
    {
        player = t;
        radius = s;
    }
}
