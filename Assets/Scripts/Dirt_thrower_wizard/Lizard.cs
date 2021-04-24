using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizard : MonoBehaviour
{
    //Obtiene el transform spawner, desde donde se van a instanciar los proyectiles
    public Transform spawnerProjectile;
    //Obtiene el objeto projectile de la carpeta Prefabs
    public GameObject dirtProjectile;
    //Indica el tiempo que pasa entre un proyectil y el siguiente
    public float shootCadenceSecs;
    //Indica si queremos que haya una repeticion de projectiles o no
    public bool autoShoot = true;
    //Indica si el lagarto puede disparar
    public bool canShoot = true;
    //Indica si me puedo destruir al chocar con el proximo objeto
    public bool destructible = false;
    //Se indica el tiempo de retardo tras el primer invoke
    private float initialTime;      
    //Indica la direction en la que se mueve el lagarto, empezamos con movimiento hacia la derecha
    private string direction = "right";
    //Indica la posicion del spawner del proyectil, como empezamos con movimiento hacia la derecha la poscion es una en concreto, si es movimiento hacia la izquierda es otra
    private Vector3 posSpawner = new Vector3(0.5f, -0.75f, 0);
    //Indica la velocidad del descenso del lagarto
    private float velocity = 5;
    private Vector3 scaleChange = new Vector3(0.75f, 0.75f, 0);
    //Indica la rotacion inicial, para perder dicha referencia al rotar al lagarto dentro de la pompa 
    private Quaternion initialTrans;


    void Start()
    {
        initialTime = shootCadenceSecs;
        //Asignamos algunos valores iniciales que iran modificandose a lo largo de la ejecucion y es necesario tenerlas almacenadas
        spawnerProjectile.transform.position = transform.position + posSpawner;
        dirtProjectile.transform.position = spawnerProjectile.transform.position + new Vector3(0, -0.15f, 0);
        dirtProjectile.transform.localScale = scaleChange;
        initialTrans = transform.rotation;

        if (autoShoot) //Esta activado el autoShoot se generan continuamente proyectiles
            InvokeRepeating("Shoot", initialTime, shootCadenceSecs);
        else //NO esta activado el autoShoot se generan un unico proyectil
            Invoke("Shoot", shootCadenceSecs);
    }

    private void Update()
    {
        //RaycastHit2D col = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(0, 1));

        if (direction == "right")
        {
            //Indicamos la traslacion (movimiento) del lagarto hacia la derecha
            transform.Translate(Vector3.right * Time.deltaTime);
            spawnerProjectile.transform.position = transform.position + posSpawner;
        }
        else if (direction == "left")
        {
            //Indicamos la traslacion (movimiento) del lagarto hacia la izquierda
            transform.Translate(Vector3.left * Time.deltaTime);
            spawnerProjectile.transform.position = new Vector3((transform.position.x + -posSpawner.x), (transform.position.y + posSpawner.y), (transform.position.z + posSpawner.z));
        }
        else if (direction == "drop")
        {
            //Asignamos la rotacion al lagarto, para generar un movimineto de caida vertical, ya que al haberlo rotado el movimiento saldra con la rotacion de la manipulacion de la pompa.
            transform.rotation = initialTrans;
            //Indicamos la traslacion (movimiento) del lagarto en descenso
            transform.Translate(Vector3.down * velocity * Time.deltaTime);
        }
        //Actualizamos la posicion tanto del spawner como del proyectil ya que el lagarto esta en movimiento continuo, y al instanciar el nuevo proyectil debe tener estas referencias
        dirtProjectile.transform.position = spawnerProjectile.position + new Vector3(0, -0.15f, 0);
    }

    //Dispara bala en la direction dada
    public void Shoot()
    {
        if (canShoot == true)
        {
            Instantiate(dirtProjectile, dirtProjectile.transform.position, Quaternion.Euler(new Vector3(0, 0, 105)));            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //En este caso obtenemos el id del layer Stage
        int stageLayer = LayerMask.NameToLayer("Stage");

        //Ha colisionado con un objetro con layer Stage
        if (collision.gameObject.layer == stageLayer)
        {
            if (destructible == false)//No puedo destruirme, ya que no he interactuado con la pompa
            {
                //Como hemos colisionado con un stage hay que rotar al lagarto para que el movimiento sea acorde a la direccion
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                //La direccion del movimiento es derecha, asi que cambia la direccion del movimiento hacia la izquierda
                if (direction == "right")
                {
                    direction = "left";
                }
                else if (direction == "left") //La direccion del movimiento es izquierda, asi que cambia la direccion del movimiento hacia la derecha
                {
                    direction = "right";
                }
            }
            else//Si puedo destruirme, ya que he interactuado con la pompa
            {
                Debug.Log("Me muero");
                //DestroyImmediate(GameObject.Find("Wizard"));
            }
        }
    }

    public void StopShooting()
    {
        canShoot = false;
        direction = "drop";
        destructible = true;
    }
}
