using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
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
    //Se indica el tiempo de retardo tras el primer invoke
    private float initialTime;      
    //Indica la direction en la que se mueve el lagarto, empezamos con movimiento hacia la derecha
    string direction = "right";
    Vector3 posSpawner = new Vector3(0.5f, -0.5f, 0);


    void Start()
    {
        initialTime = shootCadenceSecs;
        //Asignamos la posicion del spawner en una poscion en concreto en base a la poscion del lagarto
        spawnerProjectile.transform.position = transform.position + new Vector3(0.5f, -0.5f, 0);
        dirtProjectile.transform.position = spawnerProjectile.transform.position + new Vector3(0, -0.15f, 0);

        if (autoShoot) //Esta activado el autoShoot se generan continuamente proyectiles
            InvokeRepeating("Shoot", initialTime, shootCadenceSecs);
        else //NO esta activado el autoShoot se generan un unico proyectil
            Invoke("Shoot", shootCadenceSecs);
    }

    private void Update()
    {
        //Obtenemos el nuevo escale por si haya cambiado y haya que rotar al lagarto
        if (direction == "right")
        {
            //Indicamos la traslacion (movimiento) del lagarto hacia la derecha
            transform.Translate(Vector3.right * Time.deltaTime);
            spawnerProjectile.transform.position = transform.position + posSpawner;
        }
        if (direction == "left")
        {
            //Indicamos la traslacion (movimiento) del lagarto hacia la izquierda
            transform.Translate(Vector3.left * Time.deltaTime);
            spawnerProjectile.transform.position = new Vector3((transform.position.x + -posSpawner.x), (transform.position.y + posSpawner.y), (transform.position.z + posSpawner.z));
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
    }

    public void stopShoothing()
    {
        canShoot = false;
        Debug.Log("Parar de disparar");
        return;
    }
}
