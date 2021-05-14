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
    private int contador;
    private int desapariciones;

    private Transform player; //Posicion del jugador
    private float radius; //Distancia a la que veo al jugador

    Vector3 dirVector;
    RaycastHit2D hitStage;
    bool hitWall;
    enum Status {Stop, Movement, Drop, Die};
    Status status;
    enum Direction { Right, Left};
    Direction direction;

    void Start()
    {
        //Obtenemos el sprite del lagarto para poder hacer uso del volteo (flip)
        this.sprite = GetComponent<SpriteRenderer>();
        this.initialTime = shootCadenceSecs;
        this.initialTrans = transform.rotation;
        this.dirVector = Vector2.zero;
        this.hitWall = false;
        this.status = Status.Stop;
        this.destructible = false;
     }

    private void Update()
    {
        if (this.status == Status.Stop)
        {
            Debug.Log(this.gameObject.name + ": Stop");
            this.transform.Translate(Vector3.zero);

            if (this.player != null)
            {
                this.status = Status.Movement;
                if (this.sprite.flipX == false)
                {
                    this.direction = Direction.Right;
                }
                else
                {
                    this.direction = Direction.Left;
                }
            }
        }
        else if (this.status == Status.Movement)
        {
            Debug.Log(this.gameObject.name + ": Movement");
            movimiento(this.direction);
            Shoot();

            if (this.player == null)
            {
                this.status = Status.Stop;
            }
        }
        else if (this.status == Status.Drop)
        {
            Debug.Log(this.gameObject.name + ": Drop");
            //Asignamos la rotacion al lagarto, para generar un movimineto de caida vertical, ya que al haberlo rotado el movimiento saldra con la rotacion de la manipulacion de la pompa.
            this.transform.rotation = this.initialTrans;
            //Indicamos la traslacion (movimiento) del lagarto en descenso
            this.transform.Translate(Vector3.down * this.dropVelocity * Time.deltaTime);
        }
        else if (this.status == Status.Die)
        {
            this.transform.Translate(Vector3.zero);
            this.contador++;
            if (this.contador % 300 == 0)
            {
                if (this.sprite.enabled == false)
                {
                    this.sprite.enabled = true;
                }
                else
                {
                    this.sprite.enabled = false;
                }
                this.desapariciones++;
            }

            if (this.desapariciones == 3)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void movimiento(Direction direction)
    {
        this.hitStage = Physics2D.Raycast(this.foot.position, Vector2.up, 0.5f);
        Debug.DrawRay(this.foot.position, Vector2.up * 0.5f, Color.red);

        if (this.hitStage.collider != null && this.hitStage.collider.name == "Tilemap" && this.hitWall == false) //Detecta colision contra el suelo y no contra una pared
        {
            if (this.direction == Direction.Right)
            {
                //Indicamos la traslacion (movimiento) del lagarto hacia la derecha y cambio del sprite (volteo)
                this.transform.Translate(Vector3.right * Time.deltaTime);                
                this.sprite.flipX = false;
            }
            else if (direction == Direction.Left)
            {
                //Indicamos la traslacion (movimiento) del lagarto hacia la izquierda yu cambio del scripte (volteo)
                this.transform.Translate(Vector3.left * Time.deltaTime);
                this.sprite.flipX = true;
            }
        }
        else if (this.hitStage.collider == null || this.hitStage.collider.name != "Tilemap" || this.hitWall == true) //No detecta colision o suelo, pero si muro
        {            
            if (this.direction == Direction.Right)
            {
                this.direction = Direction.Left;
            }
            else if (this.direction == Direction.Left)
            {
                this.direction = Direction.Right;
            }
            this.foot.localPosition = new Vector3(-this.foot.localPosition.x, this.foot.localPosition.y, this.foot.localPosition.z);
            this.spawnerProjectile.localPosition = new Vector3(-this.spawnerProjectile.localPosition.x, this.spawnerProjectile.localPosition.y, this.spawnerProjectile.localPosition.z);
            this.hitWall = false;
        }
    }

    //Dispara bala en la direction dada
    public void Shoot()
    {        
        if (Time.time > this.initialTime && this.canShoot == true)
        {
            this.initialTime = Time.time + this.shootCadenceSecs;           
            Vector3 distance = this.player.position - this.spawnerProjectile.position;
            if (Mathf.Abs(distance.x) >= 0 && Mathf.Abs(distance.x) <= this.radius)
            {
                Debug.Log("Instancio proyectil del lagarto " + this.name);
                GameObject newProjectile = (GameObject) Instantiate(this.dirtProjectile, this.spawnerProjectile.position, Quaternion.Euler(new Vector3(0, 0, 0)));
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

        //Ha colisionado con un objetro con layer Stage
        if (col.gameObject.layer == stageLayer)
        {
            if (this.destructible == true)//Puedo destruirme, ya que si he interactuado con la pompa
            {
                //Animacion del lagarto
                this.status = Status.Die;
                this.contador = 0;
                this.desapariciones = 0;
            }
            else
            {
                this.hitWall = true;
            }
        }
        else if (col.gameObject.layer == bubbleLayer)
        {
            this.canShoot = false;
        }
        else if (col.gameObject.layer == projectileLayer)
        {            
            this.destructible = true;
            this.status = Status.Drop;
        }
    }

    public void isDestructible()
    {
        this.destructible = true;
        this.status = Status.Drop;
    }

    public void PositionPlayer(Transform t, float s)
    {
        player = t;
        radius = s;
    }
}
