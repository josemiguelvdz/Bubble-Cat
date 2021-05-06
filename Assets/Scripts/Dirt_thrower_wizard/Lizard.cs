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
    private string direction;
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
    bool hitWall;

    void Start()
    {
        //Obtenemos el sprite del lagarto para poder hacer uso del volteo (flip)
        sprite = GetComponent<SpriteRenderer>();
        initialTime = shootCadenceSecs;
        initialTrans = transform.rotation;
        dir = Vector2.zero;
        hitWall = false;
        direction = "right";
    }

    private void Update()
    {
        if (String.IsNullOrEmpty(direction))
        {
            killLizard();
        }
        else
        {
            //Detectamos al jugador y trazamos un rayo hacia él
            if (player != null)
            {
                movimiento(direction);
                Shoot();
            }
        }
    }

    private void movimiento(string direction)
    {
        if (this.direction != "drop")//No tiene que caerse el lagarto
        {
            hitStage = Physics2D.Raycast(foot.position, Vector2.up, 0.5f);
            Debug.DrawRay(foot.position, Vector2.up * 0.5f, Color.red);

            if (hitStage.collider != null && (hitStage.collider.name == "Tilemap" || hitWall == false)) //Detecta suelo
            {
                if (this.direction == "right")
                {
                    //Indicamos la traslacion (movimiento) del lagarto hacia la derecha y cambio del sprite (volteo)
                    transform.Translate(Vector3.right * Time.deltaTime);
                    sprite.flipX = false;
                }
                else if (this.direction == "left")
                {
                    //Indicamos la traslacion (movimiento) del lagarto hacia la izquierda yu cambio del scripte (volteo)
                    transform.Translate(Vector3.left * Time.deltaTime);
                    sprite.flipX = true;
                }
            }
            else if (hitStage.collider == null || hitStage.collider.name != "Tilemap") //No detecta suelo
            {
                if (this.direction == "right")
                {
                    this.direction = "left";
                    //foot.position = new Vector3(-foot.position.x, foot.position.y, foot.position.z);
                }
                else if (this.direction == "left")
                {
                    this.direction = "right";
                    //foot.position = new Vector3(foot.position.x, foot.position.y, foot.position.z);
                }
                hitWall = false;
                foot.localPosition = new Vector3(-foot.localPosition.x, foot.localPosition.y, foot.localPosition.z);
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

    //Dispara bala en la direction dada
    public void Shoot()
    {
        if (Time.time > initialTime && canShoot == true)
        {
            initialTime = Time.time + shootCadenceSecs;         
            Vector3 distance = player.position - spawnerProjectile.position;
            if (Mathf.Abs(distance.x) >= 0 && Mathf.Abs(distance.x) <= radius)
            {
                Instantiate(dirtProjectile, spawnerProjectile.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                setDirectionDirtProjectile(distance);
            }
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
