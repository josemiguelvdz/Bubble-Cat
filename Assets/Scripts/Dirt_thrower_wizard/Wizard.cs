using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    //Obtiene el objeto projectile de la carpeta Prefabs
    public GameObject dirtProjectile;
    //Indica el tiempo que pasa entre un proyectil y el siguiente
    public float shootCadenceSecs;
    //Indica si queremos que haya una repeticion de projectiles o no
    public bool autoShoot;
    //Se indica el tiempo de retardo tras el primer invoke
    private float initialTime;  
    //Indica si el lagarto puede disparar
    private bool canShoot = true;

    
    Rigidbody2D rb;
    int time = 0;
    bool cambioDireccion = false;

    void Start()
    {
        initialTime = shootCadenceSecs;

        if (autoShoot)
            InvokeRepeating("Shoot", initialTime, shootCadenceSecs);
        else
            Invoke("Shoot", shootCadenceSecs);

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.right);
    }

    private void Update()
    {
        /*time = time + 1;
        Debug.Log(time);
        if (time % 1000 == 0)
        {                    
            if (cambioDireccion == false)
            {
                rb.AddForce(Vector2.zero);
                cambioDireccion = true;
                Debug.Log("Me muevo a la derecha");
            }
            else
            {
                rb.AddForce(Vector2.zero);
                cambioDireccion = false;
                Debug.Log("Me muevo a la izquierda");
            }
        }*/                
    }

    //Dispara bala en la direccion dada
    public void Shoot()
    {
        if (canShoot)
        {
            Instantiate(dirtProjectile, dirtProjectile.transform.position, Quaternion.Euler(new Vector3(0, 0, 105)));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground"))
            {
            rb.AddForce(Vector2.left);
            }
        else if (collision.gameObject.CompareTag("Wall"))
        { Debug.Log("He chocado muro"); }
    }
}
