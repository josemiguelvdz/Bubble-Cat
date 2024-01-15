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
    public bool destructible;
    //Obtenemos variables necesarias para el recalculo contionuo de los diferentes elementos
    //Se indica el tiempo de retardo tras el primer invoke
    private float initialTime;
    //Indica la velocidad del descenso del lagarto
    private float dropVelocity = 5;
    //Indica la rotacion inicial, para perder dicha referencia al rotar al lagarto dentro de la pompa 
    private Quaternion initialTrans;

    private SpriteRenderer sprite;

    private Transform player; //Posicion del jugador
    private float radius; //Distancia a la que veo al jugador

    RaycastHit2D hitStage;
    bool hitWall;
    enum Status {Stop, Movement, Drop, Die};
    Status status;
    enum Direction { Right, Left};
    Direction direction;
    Animator anim;

    void Start()
    {
        //Obtenemos el sprite del lagarto para poder hacer uso del volteo (flip)
        sprite = GetComponent<SpriteRenderer>();
        initialTime = shootCadenceSecs;
        initialTrans = transform.rotation;
        hitWall = false;
        status = Status.Stop;
        destructible = false;
        anim = GetComponent<Animator>();
     }

    private void Update()
    {
        if (status == Status.Stop)
        {
            anim.SetBool("Mov", false);
            transform.Translate(Vector3.zero);

            if (player != null)
            {
                status = Status.Movement;
                if (!sprite.flipX)
                {
                    direction = Direction.Right;
                }
                else
                {
                    direction = Direction.Left;
                }
            }
        }
        else if (status == Status.Movement)
        {
            anim.SetBool("Mov",true);
            Debug.Log(this.gameObject.name + ": Movement");
            movimiento(this.direction);
            Shoot();

            if (!player)
            {
                status = Status.Stop;
            }
        }
        else if (status == Status.Drop)
        {
            Destroy(gameObject.GetComponent<Damageable>());
            //Asignamos la rotacion al lagarto, para generar un movimineto de caida vertical, ya que al haberlo rotado el movimiento saldra con la rotacion de la manipulacion de la pompa.
            transform.rotation = initialTrans;
            //Indicamos la traslacion (movimiento) del lagarto en descenso
            transform.Translate(Vector3.down * this.dropVelocity * Time.deltaTime);
        }
        else if (status == Status.Die)
        {
            anim.SetBool("Mov", false);
            anim.SetBool("Death", true);
            transform.Translate(Vector3.zero);

            gameObject.GetComponent<EnemyHealth>().KillEnemy();
        }
    }

    private void movimiento(Direction direction)
    {
        hitStage = Physics2D.Raycast(foot.position, Vector2.up, 0.5f);
        Debug.DrawRay(foot.position, Vector2.up * 0.5f, Color.red);

        if (!hitStage.collider && hitStage.collider.name == "Tilemap" && !hitWall) //Detecta colision contra el suelo y no contra una pared
        {
            if (direction == Direction.Right)
            {
                //Indicamos la traslacion (movimiento) del lagarto hacia la derecha y cambio del sprite (volteo)
                transform.Translate(Vector3.right * Time.deltaTime);                
                sprite.flipX = false;
            }
            else if (direction == Direction.Left)
            {
                //Indicamos la traslacion (movimiento) del lagarto hacia la izquierda yu cambio del scripte (volteo)
                transform.Translate(Vector3.left * Time.deltaTime);
                sprite.flipX = true;
            }
        }
        else if (!hitStage.collider || hitStage.collider.name != "Tilemap" || hitWall) //No detecta colision o suelo, pero si muro
        {            
            if (direction == Direction.Right)
            {
                direction = Direction.Left;
            }
            else if (direction == Direction.Left)
            {
                direction = Direction.Right;
            }
            foot.localPosition = new Vector3(-foot.localPosition.x, foot.localPosition.y, foot.localPosition.z);
            spawnerProjectile.localPosition = new Vector3(-spawnerProjectile.localPosition.x, spawnerProjectile.localPosition.y, this.spawnerProjectile.localPosition.z);
            hitWall = false;
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
                Debug.Log("Instancio proyectil del lagarto " + name);
                float x = player.position.x - spawnerProjectile.position.x;
                float y = player.position.y - spawnerProjectile.position.y;
                float angle = Mathf.Atan2(y, x) * 180 / Mathf.PI;
                float AbsAngle = 360 - Mathf.Abs(angle);
                Debug.Log("Angle: " + angle);
                Debug.Log("AbsAngle: " + AbsAngle);
                GameObject newProjectile = Instantiate(dirtProjectile, spawnerProjectile.position, Quaternion.Euler(new Vector3(0, 0, AbsAngle)));
                // accedemos al script con los valores iniciales
                Dirt_projectile script = newProjectile.GetComponent<Dirt_projectile>();
                script.setVelocity(distance);
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //En este caso obtenemos el id del layer Stage
        int stageLayer = LayerMask.NameToLayer("Stage");
        int bubbleLayer = LayerMask.NameToLayer("Bubble");
        int projectileLayer = LayerMask.NameToLayer("Proyectile");
        int meleeLayer = LayerMask.NameToLayer("Melee");

            //Ha colisionado con un objetro con layer Stage
        if (col.gameObject.layer == stageLayer)
        {
            if (!destructible)
                hitWall = true;
        }
        else if (col.gameObject.layer == bubbleLayer)
        {
            canShoot = false;
        }
        else if (col.gameObject.layer == projectileLayer)
        {
            canShoot = false;
            destructible = true;
            status = Status.Drop;
        }
    }

    public void Fall()
    {
        destructible = true;
        status = Status.Drop;
    }

    public void PositionPlayer(Transform t, float s)
    {
        player = t;
        radius = s;
    }
}
